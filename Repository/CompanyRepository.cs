using Contracts;
using Entities.Models;

namespace Repository
{
    public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
    {
        public CompanyRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public IEnumerable<Company> GetAllCompanies(bool trackChanges) =>
            FindAll(trackChanges)
                .OrderBy(company => company.Name)
                .ToList(); // runs the query

        public Company? GetCompany(Guid companyId, bool trackChanges) =>
            // #TODO is new Company a good way to handle null return?
            FindByCondition(company => 
                company.Id.Equals(companyId), trackChanges)
                .SingleOrDefault();
    }
}
