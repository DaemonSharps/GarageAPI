using GarageDataBase.DTO;
using GarageDataBase.Mapping;
using GarageDataBase.Tables;
using Microsoft.EntityFrameworkCore;

namespace GarageDataBase.Extentions;

public static partial class GarageDBContextExtentions
{
    public static async Task<User> GetUser(this GarageDBContext dBContext, string email, CancellationToken cancellationToken = default)
    {
        var user = await dBContext
            .Users
            .Include(c => c.State)
            .FirstOrDefaultAsync(c => c.Email == email, cancellationToken);
        return MapperHelper.Map<User>(user);
    }

    public static async Task<User> CreateUser(
        this GarageDBContext dBContext,
        string email,
        string firstName,
        string secondName,
        string lastName,
        long stateId = 1,
        CancellationToken cancellationToken = default)
    {
        var userToCreate = new UserTable
        {
            Email = email,
            StateId = stateId,
            FirstName = firstName,
            LastName = lastName,
            SecondName = secondName
        };

        var userEntry = dBContext.Users.Add(userToCreate);
        await userEntry.Reference(c => c.State).LoadAsync(cancellationToken);
        await dBContext.SaveChangesAsync(cancellationToken);
        return MapperHelper.Map<User>(userEntry.Entity);

    }

    public static async Task<List<User>> GetUsersByFilter(
        this GarageDBContext dBContext,
        int page,
        int perPage,
        string email = null,
        string firstName = null,
        string secondName = null,
        string lastName = null,
        long visitCount = 0,
        long stateId = 0,
        CancellationToken cancellationToken = default)
    {
        var usersQuerry = dBContext
            .Users
            .Include(c => c.State)
            .AsQueryable();

        if (!string.IsNullOrEmpty(email))
            usersQuerry = usersQuerry.Where(c => c.Email == email);
        if (!string.IsNullOrEmpty(firstName))
            usersQuerry = usersQuerry.Where(c => c.FirstName == firstName);
        if (!string.IsNullOrEmpty(secondName))
            usersQuerry = usersQuerry.Where(c => c.SecondName == secondName);
        if (!string.IsNullOrEmpty(lastName))
            usersQuerry = usersQuerry.Where(c => c.LastName == lastName);
        if (visitCount != 0)
            usersQuerry = usersQuerry.Where(c => c.VisitCount == visitCount);
        if (stateId != 0)
            usersQuerry = usersQuerry.Where(c => c.StateId == stateId);

        var takeFromPage = (page - 1) * perPage;

        var users = await usersQuerry
            .Skip(takeFromPage)
            .Take(perPage)
            .ToArrayAsync(cancellationToken);

        return MapperHelper.Map<List<User>>(users);
    }
}

