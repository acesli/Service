using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TechArt.Base;
using TechArt.Common.DataContracts.General;

namespace TechArt.ServiceModel
{
    public class CatchExceptionOperationInvoker : IOperationInvoker
    {
        private readonly string _operationName;
        private readonly IOperationInvoker _baseInvoker;

        #region Construct
        public CatchExceptionOperationInvoker(IOperationInvoker baseInvoker,
            string operationName)
        {
            this._baseInvoker = baseInvoker;
            this._operationName = operationName;
        }

        #endregion
        
        #region Implement Mothods

        public object[] AllocateInputs()
        {
            return _baseInvoker.AllocateInputs();
        }

        public object Invoke(object instance, object[] inputs, out object[] outputs)
        {
            try
            {
                return _baseInvoker.Invoke(instance, inputs, out outputs);
            }
            catch (Exception ex)
            {
                outputs = new object[0];
                return CatchException(instance, ex);
            }
        }


        public IAsyncResult InvokeBegin(object instance, object[] inputs, AsyncCallback callback, object state)
        {
            return _baseInvoker.InvokeBegin(instance, inputs, callback, state);
        }

        public object InvokeEnd(object instance, out object[] outputs, IAsyncResult result)
        {
            try
            {
                return _baseInvoker.InvokeEnd(instance, out outputs, result);
            }
            catch (Exception ex)
            {
                outputs = new object[0];
                return CatchException(instance, ex);
            }
        }

        public bool IsSynchronous => _baseInvoker.IsSynchronous;

        #endregion

        #region Logging Error

        private object CatchException(object instance, Exception ex)
        {
            //添加Logging
            //TODO

            //判断是否为DC_Result类型的返回值
            //如果是，返回Success=false ,ErrorCode=
            MethodInfo methodInfo = instance.GetType().GetMethods().FirstOrDefault(
                x => string.Equals(x.Name, _operationName));
            var returnType = methodInfo.ReturnType;
            if (typeof(DC_Result).IsAssignableFrom(returnType))
            {
                DC_Result DC_Result = Activator.CreateInstance<DC_Result>();
                DC_Result.Success = false;
                DC_Result.ErrorCode = DC_ErrorCode.Exception;
                DC_Result.Message = ex.Message;
                return DC_Result;
            }
            else
            {
                return null;
            }
        }

        #endregion
    }
}
