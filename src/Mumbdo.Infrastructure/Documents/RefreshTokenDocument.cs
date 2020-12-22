using System;
using Convey.Types;

namespace Mumbdo.Infrastructure.Documents
{
    public class RefreshTokenDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; private set; }
        
        public string Token { get; private set; }
        
        public Guid UserId { get; private set; }
        
        public DateTime Expires { get; private set; }

        public RefreshTokenDocument(Guid id, string token, Guid userId, DateTime expires)
        {
            Id = id;
            Token = token;
            UserId = userId;
            Expires = expires;
        }

        public RefreshTokenDocument()
        {
            
        }
    }
}