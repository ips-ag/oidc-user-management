## Security model
Following diagram depicts model entities and relationships.

```mermaid
classDiagram
    class Permission {
        +string Id
        +string Name
        +string Description
    }
    Resource "*" <--> "*" Permission: Resource
    Permission "1" <.. "*" PermissionAssignment: Permission
    class Role {
        +string Id
        +string Name        
    }
    class PermissionAssignment {
        +string Id
        +Permission Permission
        +Role Role
    }
    Role "1" *-- "*" PermissionAssignment: Role
    Role "1" <.. "*" RoleAssignment: Role
    class User {
        +string Id
        +string Email
    }
    User "1" *-- "*" RoleAssignment: Roles
    class Resource {
        +string Id
        +string Name
        +string Description
        +string URL
    }    
    class RoleAssignment {
        +string Id
        +User User
        +Role Role
    }
```

### Resource
Entity requiring authenticated access.
### Permission
Unit of access for individual entities of a resource. Used to define fine-grained approval to use resource.
### Role
Collection of permissions, defined by common business or technical requirements.
### User
Entity representing system client. Can be assigned roles.
