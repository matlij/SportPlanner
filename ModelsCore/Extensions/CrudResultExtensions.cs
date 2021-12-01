using ModelsCore.Enums;

namespace ModelsCore.Extensions
{
    public static class CrudResultExtensions
    {
        public static bool IsPositiveResult(this CrudResult crudResult)
        {
            return crudResult == CrudResult.Ok || crudResult == CrudResult.AlreadyExists;
        }
    }
}
