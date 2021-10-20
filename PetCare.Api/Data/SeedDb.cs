using PetCare.Api.Data.Entities;
using System.Linq;
using System.Threading.Tasks;
using PetCare.Api.Helpers;
using PetCare.Common.Enums;

namespace PetCare.Api.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckPetTypesAsync();
            await CheckRacesAsync();
            await CheckMedicinesAsync();
            await CheckDocumentTypesAsync();
            await CheckProceduresAsync();
            await CheckRolesAsycn();
            await CheckUserAsync("1010", "Luis", "Salazar", "luis@yopmail.com", "311 322 4620", "Calle Luna Calle Sol", UserType.Admin);
            await CheckUserAsync("2020", "Juan", "Zuluaga", "zulu@yopmail.com", "311 322 4620", "Calle Luna Calle Sol", UserType.User);
            await CheckUserAsync("3030", "Ledys", "Bedoya", "ledys@yopmail.com", "311 322 4620", "Calle Luna Calle Sol", UserType.User);
        }

        private async Task CheckProceduresAsync()
        {
            if (!_context.Procedures.Any())
            {
                _context.Procedures.Add(new Procedure { Price = 10000, Description = "Extracción de sangre" });
                _context.Procedures.Add(new Procedure { Price = 10000, Description = "Profilaxis" });
                _context.Procedures.Add(new Procedure { Price = 10000, Description = "Corte de pelo" });
                _context.Procedures.Add(new Procedure { Price = 10000, Description = "Revisión general" });
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckUserAsync(string document, string firstName, string lastName, string email, string phoneNumber, string address, UserType userType)
        {
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                user = new User
                {
                    Address = address,
                    Document = document,
                    DocumentType = _context.DocumentTypes.FirstOrDefault(x => x.Description == "Cédula"),
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName,
                    PhoneNumber = phoneNumber,
                    UserName = email,
                    UserType = userType
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());

                string token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);
            }
        }

        private async Task CheckRolesAsycn()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString());
        }

        private async Task CheckDocumentTypesAsync()
        {
            if (!_context.DocumentTypes.Any())
            {
                _context.DocumentTypes.Add(new DocumentType { Description = "Cédula" });
                _context.DocumentTypes.Add(new DocumentType { Description = "Tarjeta de Identidad" });
                _context.DocumentTypes.Add(new DocumentType { Description = "NIT" });
                _context.DocumentTypes.Add(new DocumentType { Description = "Pasaporte" });
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckRacesAsync()
        {
            if (!_context.Races.Any())
            {
                _context.Races.Add(new Race { Description = "Pastor Alemán" });
                _context.Races.Add(new Race { Description = "Pomerania" });
                _context.Races.Add(new Race { Description = "Golden Retriever" });
                _context.Races.Add(new Race { Description = "Bulldog" });
                _context.Races.Add(new Race { Description = "Pit Bull" });
                _context.Races.Add(new Race { Description = "Doberman" });
                _context.Races.Add(new Race { Description = "Rottweiler" });
                _context.Races.Add(new Race { Description = "Gran Danés" });
                _context.Races.Add(new Race { Description = "Bull Terrier" });
                _context.Races.Add(new Race { Description = "Akita Inu" });
                _context.Races.Add(new Race { Description = "Persa" });
                _context.Races.Add(new Race { Description = "Bengala" });
                _context.Races.Add(new Race { Description = "Siamés" });
                _context.Races.Add(new Race { Description = "Esfinge" });
                _context.Races.Add(new Race { Description = "Angora" });
                _context.Races.Add(new Race { Description = "Fold Escocés" });
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckMedicinesAsync()
        {
            if (!_context.Medicines.Any())
            {
                _context.Medicines.Add(new Medicine { Price = 10000, Description = "Probioticos-Probiocat" });
                _context.Medicines.Add(new Medicine { Price = 10000, Description = "Antiparasitarios Internos-Endogard" });
                _context.Medicines.Add(new Medicine { Price = 10000, Description = "Probioticos-Probiodog" });
                _context.Medicines.Add(new Medicine { Price = 10000, Description = "Dermantologico-Orenda" });
                _context.Medicines.Add(new Medicine { Price = 10000, Description = "Dentyfarm" });
                _context.Medicines.Add(new Medicine { Price = 10000, Description = "Otico-Gentax-Oftálmico" });
                _context.Medicines.Add(new Medicine { Price = 10000, Description = "Canatox" });
                _context.Medicines.Add(new Medicine { Price = 10000, Description = "Antiparasitarios Externos" });
                _context.Medicines.Add(new Medicine { Price = 10000, Description = "Otico-Otofarm Basic" });
                _context.Medicines.Add(new Medicine { Price = 10000, Description = "Nutraceutico F Hairball" });
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckPetTypesAsync()
        {
            if (!_context.PetTypes.Any())
            {
                _context.PetTypes.Add(new PetType { Description = "Perro" });
                _context.PetTypes.Add(new PetType { Description = "Gato" });
                _context.PetTypes.Add(new PetType { Description = "Erizo" });
                await _context.SaveChangesAsync();
            }
        }
    }
}
