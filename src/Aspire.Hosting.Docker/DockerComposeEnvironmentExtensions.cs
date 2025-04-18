// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Aspire.Hosting.ApplicationModel;
using Aspire.Hosting.Docker;
using Aspire.Hosting.Lifecycle;

namespace Aspire.Hosting;

/// <summary>
/// Provides extension methods for adding Docker Compose environment resources to the application model.
/// </summary>
public static class DockerComposeEnvironmentExtensions
{
    /// <summary>
    /// Adds a Docker Compose environment to the application model.
    /// </summary>
    /// <param name="builder">The <see cref="IDistributedApplicationBuilder"/>.</param>
    /// <param name="name">The name of the Docker Compose environment resource.</param>
    /// <returns>A reference to the <see cref="IResourceBuilder{DockerComposeEnvironmentResource}"/>.</returns>
    public static IResourceBuilder<DockerComposeEnvironmentResource> AddDockerComposeEnvironment(
        this IDistributedApplicationBuilder builder,
        string name)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentException.ThrowIfNullOrEmpty(name);

        var resource = new DockerComposeEnvironmentResource(name);
        builder.Services.TryAddLifecycleHook<DockerComposeInfrastructure>();
        builder.AddDockerComposePublisher(name);
        if (builder.ExecutionContext.IsRunMode)
        {

            // Return a builder that isn't added to the top-level application builder
            // so it doesn't surface as a resource.
            return builder.CreateResourceBuilder(resource);

        }
        return builder.AddResource(resource);
    }
}
