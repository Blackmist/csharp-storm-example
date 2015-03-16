﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using Microsoft.SCP;
using Microsoft.SCP.Rpc.Generated;

namespace WordCount
{
    public class CowBell : ISCPBolt
    {
        private Context ctx;

        public CowBell(Context ctx)
        {
            Context.Logger.Info("Cowbell bolt initialization");
            this.ctx = ctx;

            // Declare Input and Output schemas
            Dictionary<string, List<Type>> inputSchema = new Dictionary<string, List<Type>>();
            // Input contains a tuple with a boolean field
            inputSchema.Add("cowbell", new List<Type>() { typeof(bool) });
            Dictionary<string, List<Type>> outputSchema = new Dictionary<string, List<Type>>();
            // Outbound contains a tuple with a string field (so we can see results)
            outputSchema.Add("default", new List<Type>() { typeof(string) });
        }

        public static CowBell Get(Context ctx, Dictionary<string, Object> parms)
        {
            return new CowBell(ctx);
        }

        public void Execute(SCPTuple tuple)
        {
            bool cowBell = tuple.GetBoolean(0);
            if(cowBell)
            {
                this.ctx.Emit(new Values("Ding Ding!"));
            }
        }
    }
}
