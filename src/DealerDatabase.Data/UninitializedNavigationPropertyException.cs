using System.Runtime.CompilerServices;

namespace DealerDatabase.Data.Entities;

public class UninitializedNavigationPropertyException(Type ownerType, [CallerMemberName] string propertyName = null!)
	: Exception($"Navigation property {ownerType.FullName}.{propertyName} has not been initialized. Make sure IQueryable.Include is called with this property.");