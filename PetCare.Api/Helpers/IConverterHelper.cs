using PetCare.Api.Data.Entities;
using PetCare.Api.Models;
using System;
using System.Threading.Tasks;

namespace PetCare.Api.Helpers
{
    public interface IConverterHelper
    {
        Task<User> ToUserAsync(UserViewModel model, Guid imageId, bool isNew);

        UserViewModel ToUserViewModel(User user);

        Task<Pet> ToPetAsync(PetViewModel model, bool isNew);

        PetViewModel ToPetViewModel(Pet pet);

        Task<Detail> ToDetailAsync(DetailViewModel model, bool isNew);

        Task<MedicalFormula> ToFormulaAsync(MedicalFormulaViewModel model, bool isNew);

        DetailViewModel ToDetailViewModel(Detail detail);

        MedicalFormulaViewModel ToFormulaViewModel(MedicalFormula formula);
    }
}
