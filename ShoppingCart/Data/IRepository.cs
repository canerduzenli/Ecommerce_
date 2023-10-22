namespace ShoppingCart.Data
{
    public interface IRepository<T, TKey> where T : class
    {
        public T Get(TKey id);
        public ICollection<T> GetAll();
        public T Create(T entity);
        public T Update(T entity);
        public void Delete(T entity);


    }
}