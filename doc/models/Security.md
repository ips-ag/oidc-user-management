```mermaid
classDiagram
    class Permission {
        +string Id
        +string Name
        +Application Application
    }
    Application "1" *-- "*" Permission: Application
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
    class Organization {
        +string Id
        +string Name
    }
    Organization "1" <.. "*" RoleAssignment: Organization
    Organization "1" <.. "*" OrganizationAssignment: Organization
    User "1" <.. "1..*" OrganizationAssignment: User
    class Application {
        +string Id
        +string Name
        +string Description
        +string URL
    }    
    class RoleAssignment {
        +string Id
        +User User
        +Role Role
        +Organization Organization
    }
    class OrganizationAssignment {
        +string Id
        +User User
        +Organization Organization
    }
```
