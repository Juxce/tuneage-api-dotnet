using System;

namespace Tuneage.Domain.Entities
{
    public class Recording
    {
        public virtual int Id
        {
            get;
            set;
        }

        public virtual int SongId
        {
            get;
            set;
        }

        public virtual int ArtistId
        {
            get;
            set;
        }

        public virtual int RecordingTypeId
        {
            get;
            set;
        }

        public virtual string Version
        {
            get;
            set;
        }

        public virtual DateTime RecordedOn
        {
            get;
            set;
        }

        public virtual bool IsExplicit
        {
            get;
            set;
        }

        public virtual decimal CredScoreSum
        {
            get;
            set;
        }
        
        public virtual string TitleOverride
        {
            get;
            set;
        }

        public virtual string SubtitleOverride
        {
            get;
            set;
        }

        public virtual Credit[] Credits
        {
            get;
            set;
        }

        public virtual Track[] Tracks
        {
            get;
            set;
        }

        public virtual Cred[] Creds
        {
            get;
            set;
        }

        public virtual Song Song
        {
            get;
            set;
        }

        public virtual Artist Artist
        {
            get;
            set;
        }

        public virtual RecordingType RecordingType
        {
            get;
            set;
        }
    }
}