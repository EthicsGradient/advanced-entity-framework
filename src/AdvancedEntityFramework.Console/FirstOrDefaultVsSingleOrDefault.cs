using BenchmarkDotNet.Attributes;

namespace AdvancedEntityFramework.Console
{
    public class FirstOrDefaultVsSingleOrDefault
    {
        private readonly EntityFrameworkRepository _repository;

        public FirstOrDefaultVsSingleOrDefault()
        {
            _repository = new EntityFrameworkRepository();
        }

        [Benchmark]
        public void FirstOrDefault() => _repository.FirstOrDefault();
        [Benchmark]
        public void FirstOrDefaultAsNoTracking() => _repository.FirstOrDefaultAsNoTracking();
        [Benchmark]
        public void SingleOrDefault() => _repository.SingleOrDefault();
        [Benchmark]
        public void SingleOrDefaultAsNoTracking() => _repository.SingleOrDefaultAsNoTracking();
    }
}
