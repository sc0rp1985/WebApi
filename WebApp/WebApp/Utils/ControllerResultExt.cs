using BLL;
using Common;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace WebApp.Utils
{
    public static class ControllerResultExt
    {
        public static IActionResult OperationResultToActionResult(this ControllerBase controller, OperationResult op)
        {
            if (op.OperationStatus == OperationStatusEnum.Succesfully)
                return controller.Ok();
            else if (op.OperationStatus == OperationStatusEnum.BadRequest)
                return controller.BadRequest(op.Message);
            else if (op.OperationStatus == OperationStatusEnum.NotFound)
                return controller.NotFound(op.Message);
            else if (op.OperationStatus == OperationStatusEnum.Error)
                return  controller.StatusCode(StatusCodes.Status500InternalServerError, op.Message);
            return controller.Ok();            
        }
    }
}
