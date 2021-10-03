# Example Microservices Application

This application consists of 2 .Net Core APIs using MSSQL as a persistant storage layer. Both services expose a RESTful interface for external access and communicate internally using RabbitMQ as a Message Bus and gRPC.

The entier application is hosted in Kubernetes and can be deployed locally and in multiple cloud environments. Currently the application is configured with Development and Production configuration files.

For easier local management and deployment Tilt is used to maintain the local K8S cluster which allows for auto reloading of the application when changes are made.

## Access

### Local

You can run both APIs locally, they are configured as follows:

**Platform Service**

Ports: 5000 (http) and 5001 (https)

**Command Service**

Ports: 6000 (http) and 6001 (https)

### Containers

Access to the application running inside the cluster is via the hostname `acme.com`. You will need to add this hostname to your hosts file.

```
acme.com 127.0.0.1
```

## Message Bus

The use of the Message Bus allows for [Eventual Consistency](https://en.wikipedia.org/wiki/Eventual_consistency) data model. Each API (microservice) maintains a copy of the data it needs, this removes the dependency for the microservices to communicate a lot. This increases availability and reliability and improves performance.

## References

- Project/Tutorial => https://www.youtube.com/watch?v=DgVjEo3OGBI
- https://www.docker.com/
- https://kubernetes.io/
- https://tilt.dev/
