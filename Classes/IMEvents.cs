using System;

namespace Astrodon.Classes {

    public enum IMError : byte {
        TooUserName = CommClient.IM_TooUsername,
        TooPassword = CommClient.IM_TooPassword,
        Exists = CommClient.IM_Exists,
        NoExists = CommClient.IM_NoExists,
        WrongPassword = CommClient.IM_WrongPass
    }

    public class IMErrorEventArgs : EventArgs {
        private IMError err;

        public IMErrorEventArgs(IMError error) {
            this.err = error;
        }

        public IMError Error {
            get { return err; }
        }
    }

    public class IMAvailEventArgs : EventArgs {
        private string user;
        private bool avail;

        public IMAvailEventArgs(string user, bool avail) {
            this.user = user;
            this.avail = avail;
        }

        public string UserName {
            get { return user; }
        }

        public bool IsAvailable {
            get { return avail; }
        }
    }

    public class IMReceivedEventArgs : EventArgs {
        private string msg;

        public IMReceivedEventArgs(string msg) {
            this.msg = msg;
        }

        public string Message { get { return msg; } }
    }

    public delegate void IMErrorEventHandler(object sender, IMErrorEventArgs e);

    public delegate void IMAvailEventHandler(object sender, IMAvailEventArgs e);

    public delegate void IMReceivedEventHandler(object sender, IMReceivedEventArgs e);
}