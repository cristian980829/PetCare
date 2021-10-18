using PetCare.Api.Data.Entities;
using PetCare.Api.Models;
using System;
using System.Threading.Tasks;

namespace PetCare.Api.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        private readonly DataContext _context;
        private readonly ICombosHelper _combosHelper;

        public ConverterHelper(DataContext context, ICombosHelper combosHelper)
        {
            _context = context;
            _combosHelper = combosHelper;
        }

        public async Task<Detail> ToDetailAsync(DetailViewModel model, bool isNew)
        {
            return new Detail
            {
                Id = isNew ? 0 : model.Id,
                ClinicalHistory = await _context.ClinicalHistories.FindAsync(model.ClinicalHistoryId),
                ProcedurePrice = model.ProcedurePrice,
                Procedure = await _context.Procedures.FindAsync(model.ProcedureId),
                Remarks = model.Remarks,
                MedicinePrice = model.MedicinePrice
            };
        }

        public DetailViewModel ToDetailViewModel(Detail detail)
        {
            return new DetailViewModel
            {
                ClinicalHistoryId = detail.ClinicalHistory.Id,
                Id = detail.Id,
                ProcedurePrice = detail.ProcedurePrice,
                ProcedureId = detail.Procedure.Id,
                Procedures = _combosHelper.GetComboProcedures(),
                Remarks = detail.Remarks,
                MedicinePrice = detail.MedicinePrice
            };
        }

        public async Task<User> ToUserAsync(UserViewModel model, Guid imageId, bool isNew)
        {
            return new User
            {
                Address = model.Address,
                Document = model.Document,
                DocumentType = await _context.DocumentTypes.FindAsync(model.DocumentTypeId),
                Email = model.Email,
                FirstName = model.FirstName,
                Id = isNew ? Guid.NewGuid().ToString() : model.Id,
                ImageId = imageId,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                UserName = model.Email,
                UserType = model.UserType,
            };
        }

        public UserViewModel ToUserViewModel(User user)
        {
            return new UserViewModel
            {
                Address = user.Address,
                Document = user.Document,
                DocumentTypeId = user.DocumentType.Id,
                DocumentTypes = _combosHelper.GetComboDocumentTypes(),
                Email = user.Email,
                FirstName = user.FirstName,
                Id = user.Id,
                ImageId = user.ImageId,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                UserType = user.UserType,
            };
        }

        public async Task<Pet> ToPetAsync(PetViewModel model, bool isNew)
        {
            return new Pet
            {
                Race = await _context.Races.FindAsync(model.RaceId),
                DateOfBirth = model.DateOfBirth,
                Id = isNew ? 0 : model.Id,
                Weight = model.Weight,
                Name = model.Name,
                Remarks = model.Remarks,
                PetType = await _context.PetTypes.FindAsync(model.PetTypeId)
            };
        }

        public PetViewModel ToPetViewModel(Pet pet)
        {
            return new PetViewModel
            {
                RaceId = pet.Race.Id,
                Races = _combosHelper.GetComboRaces(),
                Id = pet.Id,
                Weight = pet.Weight,
                Name = pet.Name,
                DateOfBirth = pet.DateOfBirth,
                Remarks = pet.Remarks,
                UserId = pet.User.Id,
                PetPhotos = pet.PetPhotos,
                PetTypeId = pet.PetType.Id,
                PetTypes = _combosHelper.GetComboPetTypes()
            };
        }

    }
}
