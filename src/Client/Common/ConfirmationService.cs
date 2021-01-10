using System.Threading.Tasks;
using MudBlazor.Dialog;
using Mumbdo.Web.Interfaces.Common;
using Mumbdo.Web.Shared;

namespace Mumbdo.Web.Common
{
    public class ConfirmationService : IConfirmationService
    {
        private readonly IDialogService _dialogService;

        public ConfirmationService(IDialogService dialogService)
        {
            _dialogService = dialogService;
        }
        
        public async Task<bool> ConfirmAsync(string confirmationMessage)
        {
            var dialog = _dialogService.Show<ConfirmationDialog>(confirmationMessage);
            var result = await dialog.Result;
            return !result.Cancelled;
        }
    }
}