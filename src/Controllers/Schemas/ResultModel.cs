using GarageAPI.DataBase.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GarageAPI.Controllers.Schemas
{
    public class ResultModel<T> where T: class
    {
        private ModelActions _action = ModelActions.Find;

        public string Action { 
            get 
            {
                return _action.ToString();
            }
            set
            {
                _action = value switch
                {
                    nameof(ModelActions.Find) => ModelActions.Find,
                    nameof(ModelActions.Create) => ModelActions.Create,
                    nameof(ModelActions.Get) => ModelActions.Get,
                    _ => throw new NotSupportedException("Такой метод не поддерживается")
                };
            }
        }

        public T Result { get; set; }
    }

    public enum ModelActions
    {
        Find = 1,
        Get = 2,
        Create = 3
    }
}
