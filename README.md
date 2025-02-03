```mermaid
flowchart TB
    subgraph "Development Environment"
        AD[Aspire Dashboard]
        
        subgraph "Aspire AppHost Project"
            AH[AppHost Program.cs]
        end
        
        subgraph "Service Projects"
            API[API Service]
            Web[Web Frontend]
            Redis[Redis Cache]
            Other[Other Services...]
        end
        
        AH --> |Orchestrates| API
        AH --> |Orchestrates| Web
        AH --> |Orchestrates| Redis
        AH --> |Orchestrates| Other
        
        AD --> |Monitors| API
        AD --> |Monitors| Web
        AD --> |Monitors| Redis
        AD --> |Monitors| Other
    end
    
    subgraph "Production Environment"
        subgraph "Container Platform (K8s/Cloud)"
            API2[API Service]
            Web2[Web Frontend]
            Redis2[Redis Cache]
            Other2[Other Services...]
            
            LB[Load Balancer]
            
            LB --> Web2
            Web2 --> API2
            API2 --> Redis2
            API2 --> Other2
        end
        
        subgraph "Monitoring"
            Tel[Telemetry]
            Log[Logging]
            Met[Metrics]
        end
        
        API2 --> Tel
        Web2 --> Tel
        Redis2 --> Tel
        Other2 --> Tel
        
        Tel --> Log
        Tel --> Met
    end
    
    Client1[Web Browser] --> LB
    Client2[Mobile App] --> LB
    Client3[Chrome Extension] --> LB

    style Development Environment fill:#f0f7ff,stroke:#333,stroke-width:2px
    style Production Environment fill:#fff7f0,stroke:#333,stroke-width:2px
    style Client1 fill:#FFFFF
    style Client2 fill:#FFFFF
    style Client3 fill:#FFFFF