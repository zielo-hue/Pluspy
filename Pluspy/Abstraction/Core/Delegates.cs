﻿namespace Pluspy.Core
{
    public delegate void EventHandler<in TSender, in TEventArgs>(TSender sender, TEventArgs eventArgs);
}
