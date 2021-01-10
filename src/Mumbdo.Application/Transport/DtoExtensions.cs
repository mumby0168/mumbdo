using Mumbdo.Domain.Entities;
using Mumbdo.Shared.Dtos;

namespace Mumbdo.Application.Transport
{
    public static class DtoExtensions
    {
        private static ITransferDataService _implementation = new TransferDataService();

        public static void SetMockedImplementation(ITransferDataService implementation) =>
            _implementation = implementation;

        public static void Reset() => _implementation = new TransferDataService();

        public static TaskDto AsTaskDto(this ITaskEntity task) => _implementation.AsTaskDto(task);


    }
}