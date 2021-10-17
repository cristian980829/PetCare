using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace PetCare.Api.Helpers
{
    public interface ICombosHelper
    {
        IEnumerable<SelectListItem> GetCombosDocumentTypes();

        IEnumerable<SelectListItem> GetComboProcedures();

        IEnumerable<SelectListItem> GetComboPetTypes();

        IEnumerable<SelectListItem> GetComboRaces();
    }
}
