using Helpers.Datatypes;

namespace DPA_Musicsheets.New.Builder
{
    public abstract class AbstractBuilder<T> : IBuilder<T>
        where T : new()
    {
        protected AbstractBuilder()
        {
            ToBuild = new T();
        }

        protected T ToBuild { get; set; }

        public T Build()
        {
            var temp = ToBuild;
            ToBuild = new T();

            return temp;
        }
    }
}