﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetCare.Api.Data.Entities;
using PetCare.Api.Helpers;
using PetCare.Api.Models;
using PetCare.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PetCare.Common.Models;

namespace PetCare.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly ICombosHelper _combosHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly IMailHelper _mailHelper;

        public UsersController(DataContext context, IUserHelper userHelper, ICombosHelper combosHelper, IConverterHelper converterHelper, IBlobHelper blobHelper, IMailHelper mailHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _combosHelper = combosHelper;
            _converterHelper = converterHelper;
            _blobHelper = blobHelper;
            _mailHelper = mailHelper;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Users
                .Include(x => x.DocumentType)
                .Include(x => x.Pets)
                .Where(x => x.UserType == UserType.User)
                .ToListAsync());
        }

        public IActionResult Create()
        {
            UserViewModel model = new UserViewModel
            {
                DocumentTypes = _combosHelper.GetComboDocumentTypes()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;

                if (model.ImageFile != null)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
                }

                User user = await _converterHelper.ToUserAsync(model, imageId, true);
                user.UserType = UserType.User;
                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, user.UserType.ToString());

                string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                string tokenLink = Url.Action("ConfirmEmail", "Account", new
                {
                    userid = user.Id,
                    token = myToken
                }, protocol: HttpContext.Request.Scheme);

                Response response = _mailHelper.SendMail(model.Email, "Pets - Confirmación de cuenta", $"<h1>Pets - Confirmación de cuenta</h1>" +
                    $"Para habilitar el usuario, " +
                    $"por favor hacer clic en el siguiente enlace: </br></br><a href = \"{tokenLink}\">Confirmar Email</a>");

                return RedirectToAction(nameof(Index));
            }

            model.DocumentTypes = _combosHelper.GetComboDocumentTypes();
            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            User user = await _userHelper.GetUserAsync(Guid.Parse(id));
            if (user == null)
            {
                return NotFound();
            }

            UserViewModel model = _converterHelper.ToUserViewModel(user);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = model.ImageId;
                if (model.ImageFile != null)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
                }

                User user = await _converterHelper.ToUserAsync(model, imageId, false);
                await _userHelper.UpdateUserAsync(user);
                return RedirectToAction(nameof(Index));
            }

            model.DocumentTypes = _combosHelper.GetComboDocumentTypes();
            return View(model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            User user = await _userHelper.GetUserAsync(Guid.Parse(id));
            if (user == null)
            {
                return NotFound();
            }

            await _blobHelper.DeleteBlobAsync(user.ImageId, "users");
            await _userHelper.DeleteUserAsync(user);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            User user = await _context.Users
                .Include(x => x.DocumentType)
                .Include(x => x.Pets)
                .ThenInclude(x => x.Race)
                .Include(x => x.Pets)
                .ThenInclude(x => x.PetType)
                .Include(x => x.Pets)
                .ThenInclude(x => x.PetPhotos)
                .Include(x => x.Pets)
                .ThenInclude(x => x.ClinicalHistories)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }


        public async Task<IActionResult> AddPet(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            User user = await _context.Users
                .Include(x => x.Pets)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            PetViewModel model = new PetViewModel
            {
                Races = _combosHelper.GetComboRaces(),
                UserId = user.Id,
                PetTypes = _combosHelper.GetComboPetTypes()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPet(PetViewModel petViewModel)
        {
            User user = await _context.Users
                .Include(x => x.Pets)
                .FirstOrDefaultAsync(x => x.Id == petViewModel.UserId);
            if (user == null)
            {
                return NotFound();
            }

            Guid imageId = Guid.Empty;
            if (petViewModel.ImageFile != null)
            {
                imageId = await _blobHelper.UploadBlobAsync(petViewModel.ImageFile, "petphotos");
            }

            Pet pet = await _converterHelper.ToPetAsync(petViewModel, true);
            if (pet.PetPhotos == null)
            {
                pet.PetPhotos = new List<PetPhoto>();
            }

            pet.PetPhotos.Add(new PetPhoto
            {
                ImageId = imageId
            });

            try
            {
                user.Pets.Add(pet);
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = user.Id });
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                {
                    ModelState.AddModelError(string.Empty, "Ya existe una mascota con esa placa.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                }
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }

            petViewModel.Races = _combosHelper.GetComboRaces();
            petViewModel.PetTypes = _combosHelper.GetComboPetTypes();
            return View(petViewModel);
        }

        public async Task<IActionResult> EditPet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Pet pet = await _context.Pets
                .Include(x => x.User)
                .Include(x => x.Race)
                .Include(x => x.PetType)
                .Include(x => x.PetPhotos)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (pet == null)
            {
                return NotFound();
            }

            PetViewModel model = _converterHelper.ToPetViewModel(pet);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPet(int id, PetViewModel petViewModel)
        {
            if (id != petViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Pet pet = await _converterHelper.ToPetAsync(petViewModel, false);
                    _context.Pets.Update(pet);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { id = petViewModel.UserId });
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe una mascota con esta identificación.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }

            petViewModel.Races = _combosHelper.GetComboRaces();
            petViewModel.PetTypes = _combosHelper.GetComboPetTypes();
            return View(petViewModel);
        }

        public async Task<IActionResult> DeletePet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Pet pet = await _context.Pets
                .Include(x => x.User)
                .Include(x => x.PetPhotos)
                .Include(x => x.ClinicalHistories)
                .ThenInclude(x => x.Details)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (pet == null)
            {
                return NotFound();
            }

            _context.Pets.Remove(pet);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = pet.User.Id });
        }

        public async Task<IActionResult> DeleteImagePet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PetPhoto petPhoto = await _context.PetPhotos
                .Include(x => x.Pet)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (petPhoto == null)
            {
                return NotFound();
            }

            try
            {
                await _blobHelper.DeleteBlobAsync(petPhoto.ImageId, "petphotos");
            }
            catch { }
            _context.PetPhotos.Remove(petPhoto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(EditPet), new { id = petPhoto.Pet.Id });
        }

        public async Task<IActionResult> AddPetImage(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Pet pet = await _context.Pets
                .FirstOrDefaultAsync(x => x.Id == id);
            if (pet == null)
            {
                return NotFound();
            }

            PetPhotoViewModel model = new()
            {
                PetId = pet.Id
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPetImage(PetPhotoViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "petphotos");
                Pet pet = await _context.Pets
                    .Include(x => x.PetPhotos)
                    .FirstOrDefaultAsync(x => x.Id == model.PetId);
                if (pet.PetPhotos == null)
                {
                    pet.PetPhotos = new List<PetPhoto>();
                }

                pet.PetPhotos.Add(new PetPhoto
                {
                    ImageId = imageId
                });

                _context.Pets.Update(pet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(EditPet), new { id = pet.Id });
            }

            return View(model);
        }

        public async Task<IActionResult> DetailsPet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Pet pet = await _context.Pets
                .Include(x => x.User)
                .Include(x => x.PetType)
                .Include(x => x.Race)
                .Include(x => x.PetPhotos)
                .Include(x => x.ClinicalHistories)
                .ThenInclude(x => x.Details)
                .ThenInclude(x => x.Procedure)
                .Include(x => x.ClinicalHistories)
                .ThenInclude(x => x.MedicalFormulas)
                .ThenInclude(x => x.Medicine)
                .Include(x => x.ClinicalHistories)
                .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (pet == null)
            {
                return NotFound();
            }

            return View(pet);
        }

        public async Task<IActionResult> AddClinicalHistory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Pet pet = await _context.Pets.FindAsync(id);
            if (pet == null)
            {
                return NotFound();
            }

            ClinicalHistoryViewModel model = new ClinicalHistoryViewModel
            {
                PetId = pet.Id
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddClinicalHistory(ClinicalHistoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                Pet pet = await _context.Pets
                    .Include(x => x.ClinicalHistories)
                    .FirstOrDefaultAsync(x => x.Id == model.PetId);
                if (pet == null)
                {
                    return NotFound();
                }

                User user = await _userHelper.GetUserAsync(User.Identity.Name);
                ClinicalHistory clinicalHistory = new ClinicalHistory
                {
                    Date = DateTime.UtcNow,
                    Remarks = model.Remarks,
                    User = user
                };

                if (pet.ClinicalHistories == null)
                {
                    pet.ClinicalHistories = new List<ClinicalHistory>();
                }

                pet.ClinicalHistories.Add(clinicalHistory);
                _context.Pets.Update(pet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(DetailsPet), new { id = pet.Id });
            }

            return View(model);
        }

        public async Task<IActionResult> EditClinicalHistory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ClinicalHistory clinicalHistory = await _context.ClinicalHistories
                .Include(x => x.Pet)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (clinicalHistory == null)
            {
                return NotFound();
            }

            ClinicalHistoryViewModel model = new ClinicalHistoryViewModel
            {
                Date = clinicalHistory.Date,
                Remarks = clinicalHistory.Remarks,
                PetId = clinicalHistory.Pet.Id
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditClinicalHistory(int id, ClinicalHistoryViewModel historyViewModel)
        {
            if (ModelState.IsValid)
            {
                ClinicalHistory history = await _context.ClinicalHistories.FindAsync(id);
                history.Date = historyViewModel.Date;
                history.Remarks = historyViewModel.Remarks;
                _context.ClinicalHistories.Update(history);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(DetailsPet), new { id = historyViewModel.PetId });
            }

            return View(historyViewModel);
        }

        public async Task<IActionResult> DeleteClinicalHistory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ClinicalHistory history = await _context.ClinicalHistories
                .Include(x => x.Details)
                .Include(x => x.Pet)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (history == null)
            {
                return NotFound();
            }

            _context.ClinicalHistories.Remove(history);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(DetailsPet), new { id = history.Pet.Id });
        }

        public async Task<IActionResult> DetailsClinicalHistory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ClinicalHistory history = await _context.ClinicalHistories
                .Include(x => x.Details)
                .ThenInclude(x => x.Procedure)
                .Include(x => x.Details)
                .ThenInclude(x => x.Medicine)
                .Include(x => x.Pet)
                .ThenInclude(x => x.PetPhotos)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (history == null)
            {
                return NotFound();
            }

            return View(history);
        }

        public async Task<IActionResult> AddDetail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ClinicalHistory history = await _context.ClinicalHistories.FindAsync(id);
            if (history == null)
            {
                return NotFound();
            }

            DetailViewModel model = new DetailViewModel
            {
                ClinicalHistoryId = history.Id,
                Procedures = _combosHelper.GetComboProcedures(),
                Medicines = _combosHelper.GetComboMedicines()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDetail(DetailViewModel detailViewModel)
        {
            if (ModelState.IsValid)
            {
                ClinicalHistory history = await _context.ClinicalHistories
                    .Include(x => x.Details)
                    .FirstOrDefaultAsync(x => x.Id == detailViewModel.ClinicalHistoryId);
                if (history == null)
                {
                    return NotFound();
                }

                if (history.Details == null)
                {
                    history.Details = new List<Detail>();
                }

                Detail detail = await _converterHelper.ToDetailAsync(detailViewModel, true);
                history.Details.Add(detail);
                _context.ClinicalHistories.Update(history);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(DetailsClinicalHistory), new { id = detailViewModel.ClinicalHistoryId });
            }

            detailViewModel.Procedures = _combosHelper.GetComboProcedures();
            detailViewModel.Medicines = _combosHelper.GetComboMedicines();
            return View(detailViewModel);
        }

        public async Task<IActionResult> EditDetail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Detail detail = await _context.Details
                .Include(x => x.ClinicalHistory)
                .Include(x => x.Procedure)
                .Include(x => x.ClinicalHistory)
                .Include(x => x.Medicine)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (detail == null)
            {
                return NotFound();
            }

            DetailViewModel model = _converterHelper.ToDetailViewModel(detail);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDetail(int id, DetailViewModel detailViewModel)
        {
            if (ModelState.IsValid)
            {
                Detail detail = await _converterHelper.ToDetailAsync(detailViewModel, false);
                _context.Details.Update(detail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(DetailsClinicalHistory), new { id = detailViewModel.ClinicalHistoryId });
            }

            detailViewModel.Procedures = _combosHelper.GetComboProcedures();
            detailViewModel.Medicines = _combosHelper.GetComboMedicines();
            return View(detailViewModel);
        }

        public async Task<IActionResult> DeleteDetail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Detail detail = await _context.Details
                .Include(x => x.ClinicalHistory)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (detail == null)
            {
                return NotFound();
            }

            _context.Details.Remove(detail);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(DetailsClinicalHistory), new { id = detail.ClinicalHistory.Id });
        }



        //===================================================================

        public async Task<IActionResult> FormulaClinicalHistory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ClinicalHistory history = await _context.ClinicalHistories
                .Include(x => x.MedicalFormulas)
                .ThenInclude(x => x.Medicine)
                .Include(x => x.Pet)
                .ThenInclude(x => x.PetPhotos)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (history == null)
            {
                return NotFound();
            }

            return View(history);
        }

        public async Task<IActionResult> AddFormula(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ClinicalHistory history = await _context.ClinicalHistories.FindAsync(id);
            if (history == null)
            {
                return NotFound();
            }

            MedicalFormulaViewModel model = new MedicalFormulaViewModel
            {
                ClinicalHistoryId = history.Id,
                Medicines = _combosHelper.GetComboMedicines()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFormula(MedicalFormulaViewModel formulaViewModel)
        {
            if (ModelState.IsValid)
            {
                ClinicalHistory history = await _context.ClinicalHistories
                    .Include(x => x.MedicalFormulas)
                    .FirstOrDefaultAsync(x => x.Id == formulaViewModel.ClinicalHistoryId);
                if (history == null)
                {
                    return NotFound();
                }

                if (history.MedicalFormulas == null)
                {
                    history.MedicalFormulas = new List<MedicalFormula>();
                }

                MedicalFormula formula = await _converterHelper.ToFormulaAsync(formulaViewModel, true);
                history.MedicalFormulas.Add(formula);
                _context.ClinicalHistories.Update(history);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(FormulaClinicalHistory), new { id = formulaViewModel.ClinicalHistoryId });
            }

            formulaViewModel.Medicines = _combosHelper.GetComboMedicines();
            return View(formulaViewModel);
        }

        public async Task<IActionResult> EditFormula(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MedicalFormula formula = await _context.MedicalFormulas
                .Include(x => x.ClinicalHistory)
                .Include(x => x.Medicine)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (formula == null)
            {
                return NotFound();
            }

            MedicalFormulaViewModel model = _converterHelper.ToFormulaViewModel(formula);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditFormula(int id, MedicalFormulaViewModel formulaViewModel)
        {
            if (ModelState.IsValid)
            {
                MedicalFormula formula = await _converterHelper.ToFormulaAsync(formulaViewModel, false);
                _context.MedicalFormulas.Update(formula);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(FormulaClinicalHistory), new { id = formulaViewModel.ClinicalHistoryId });
            }

            formulaViewModel.Medicines = _combosHelper.GetComboMedicines();
            return View(formulaViewModel);
        }

        public async Task<IActionResult> DeleteFormula(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MedicalFormula formula = await _context.MedicalFormulas
                .Include(x => x.ClinicalHistory)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (formula == null)
            {
                return NotFound();
            }

            _context.MedicalFormulas.Remove(formula);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(FormulaClinicalHistory), new { id = formula.ClinicalHistory.Id });
        }

    }
}
