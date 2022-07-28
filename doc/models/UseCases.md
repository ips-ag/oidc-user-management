## Use Cases

1. New resource setup, consisting of creating resource, adding permissions, creating and assigning role. In the example, **ERP** resource provides customer orders, which can be access using **orders:read** permission. We would like to create an **Order Supervisor** role, which would have said permission, and assign it to existing user **Bob**.

```mermaid
sequenceDiagram
    actor A as Administrator
    participant R as Resources
    participant P as Permissions
    participant RO as Roles
    participant U as Users
    A->>R: createResource("ERP")
    Note over R: resources:full required
    A->>P: createPermission("ERP", "orders:read")
    Note over P: permissions:full required
    A->>RO: createRole("Order Supervisor")
    Note over RO: roles:full required
    A->>RO: assignPermission("Order Supervisor", "orders:read")
    Note over RO: permission-assignments:full required
    A->>U: assignRole("Order Supervisor", "Bob")
    Note over U: role-assignments:full required
    actor B as Bob
    participant E as ERP
    B->>+E: getOrders()
    Note over E: orders:read required
    actor B as Bob
    E->>-B: orders
```

2. Onboarding of a new user by a manager. As a **User Manager**, I should be allowed to create new user **Bob** and assign him an **Order Supervisor** role. A notification of account creation would be sent to **Bob** via email, allowing him to complete registration. Once completed, he is able to access **ERP** resource orders.

```mermaid
sequenceDiagram
    actor UM as User Manager
    participant RO as Roles
    participant U as Users
    UM->>U: createUser(name: "Bob", email: "bob@example.com")
    Note over U: users:full required
    UM->>U: assignRole("Order Supervisor", "Bob")
    Note over U: role-assignments:full required
    actor B as Bob
    U->>+B: notification(registrationUrl)
    B->>-U: register()
    participant E as ERP
    B->>+E: getOrders()
    Note over E: orders:read required
    actor B as Bob
    E->>-B: orders
```

