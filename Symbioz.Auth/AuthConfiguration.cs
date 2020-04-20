using Symbioz.Core.DesignPattern.StartupEngine;
using System;
using System.Collections.Generic;
using System.IO;
using Symbioz.Core;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using YAXLib;
using Symbioz.Protocol.Types;

namespace Symbioz.Auth {
    [YAXComment("AuthServer Configuration")]
    public class AuthConfiguration : ServerConfiguration {
        public const string CONFIG_NAME = "auth.xml";

        #region Public Static

        [StartupInvoke("Configuration", StartupInvokePriority.Primitive)]
        public static void Initialize() {
            Instance = Load<AuthConfiguration>(CONFIG_NAME);
        }

        public static AuthConfiguration Instance = null;

        #endregion

        #region Public

        public int DofusProtocolVersion { get; set; }

        public sbyte VersionInstall { get; set; }

        public sbyte VersionMajor { get; set; }

        public sbyte VersionMinor { get; set; }

        public sbyte VersionRelease { get; set; }

        public sbyte VersionPatch { get; set; }

        public int VersionRevision { get; set; }

        public sbyte VersionTechnology { get; set; }

        public sbyte VersionBuildType { get; set; }

        public VersionExtended GetVersionExtended() {
            return new VersionExtended(this.VersionMajor,
                                       this.VersionMinor,
                                       this.VersionRelease,
                                       this.VersionRevision,
                                       this.VersionPatch,
                                       this.VersionBuildType,
                                       this.VersionInstall,
                                       this.VersionTechnology);
        }

        public override void Default() {
            this.DatabaseHost = "127.0.0.1";

            this.DatabaseUser = "root";

            this.DatabasePassword = string.Empty;

            this.DatabaseName = "symbioz_auth";

            this.Host = "127.0.0.1";

            this.Port = 443;

            this.ShowProtocolMessages = true;

            this.SafeRun = false;

            this.DofusProtocolVersion = 1709; // 2.34

            this.TransitionHost = "127.0.0.1";

            this.TransitionPort = 600;

            this.VersionInstall = 1;

            this.VersionTechnology = 1;

            this.VersionBuildType = 0;

            this.VersionMajor = 2;

            this.VersionMinor = 34;

            this.VersionPatch = 2;

            this.VersionRelease = 2;

            this.VersionRevision = 103887;
        }

        #endregion
    }
}