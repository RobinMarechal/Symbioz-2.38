﻿using SSync.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSync.Transition
{
    public delegate void RequestCallbackDelegate<in T>(T callbackMessage) where T : TransitionMessage;
}
