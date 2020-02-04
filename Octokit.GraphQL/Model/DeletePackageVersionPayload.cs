namespace Octokit.GraphQL.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using Octokit.GraphQL.Core;
    using Octokit.GraphQL.Core.Builders;

    /// <summary>
    /// Autogenerated return type of DeletePackageVersion
    /// </summary>
    public class DeletePackageVersionPayload : QueryableValue<DeletePackageVersionPayload>
    {
        internal DeletePackageVersionPayload(Expression expression) : base(expression)
        {
        }

        /// <summary>
        /// A unique identifier for the client performing the mutation.
        /// </summary>
        public string ClientMutationId { get; }

        /// <summary>
        /// Whether or not the operation succeeded.
        /// </summary>
        public bool? Success { get; }

        internal static DeletePackageVersionPayload Create(Expression expression)
        {
            return new DeletePackageVersionPayload(expression);
        }
    }
}