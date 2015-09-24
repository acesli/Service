using System.ComponentModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace TechArt.ServiceModel
{
    public class CatchExceptionOperationBehavior : IOperationBehavior
    {
        private bool _catchException = true;//是否捕捉异常

        #region Proporty

        [Description("Catch the exception or not.Default value is true.")]
        public bool CatchException
        {
            get { return _catchException; }
            set { _catchException = value; }
        }

        #endregion

        #region Implement IOperationBehavior

        public void Validate(OperationDescription operationDescription)
        {
        }

        public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
            if (_catchException)
            {
                dispatchOperation.Invoker = new CatchExceptionOperationInvoker(dispatchOperation.Invoker,
                    operationDescription.Name);
            }
        }

        public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
        {
        }

        public void AddBindingParameters(OperationDescription operationDescription, BindingParameterCollection bindingParameters)
        {
        }

        #endregion
    }
}