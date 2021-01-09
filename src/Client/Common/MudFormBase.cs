using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Mumbdo.Web.Common
{
    public class MudFormBase : ComponentBase
    {
        protected MudForm Form;
        
        protected bool IsFormValid { get; set; }
        
        protected string ErrorMessage { get; set; }
    }
}