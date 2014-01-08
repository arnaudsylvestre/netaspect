#region license

// Copyright (c) 2004, Rodrigo B. de Oliveira (rbo@acm.org)
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
// 
//     * Redistributions of source code must retain the above copyright notice,
//     this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright notice,
//     this list of conditions and the following disclaimer in the documentation
//     and/or other materials provided with the distribution.
//     * Neither the name of Rodrigo B. de Oliveira nor the names of its
//     contributors may be used to endorse or promote products derived from this
//     software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE
// FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
// CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
// THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

#endregion

using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace FluentAspect.Weaver.Core.Errors
{
    public class PEVerify
    {
        public void Run(string assemblyFile)
        {
            string command = null;
            string arguments = string.Empty;

            switch ((int) Environment.OSVersion.Platform)
            {
                case (int) PlatformID.Unix:
                case 128: // mono's PlatformID.Unix workaround on 1.1
                    command = "pedump";
                    arguments = "--verify all \"" + assemblyFile + "\"";
                    break;
                default: // Windows
                    command = "peverify.exe";
                    arguments = "\"" + assemblyFile + "\"";
                    break;
            }
            int exitCode = 0;
            string output = "";
            Process p = StartProcess(@".\", command, arguments);
            p.WaitForExit();
            output = p.StandardOutput.ReadToEnd();
            exitCode = p.ExitCode;
            if (0 != exitCode)
            {
                throw new Exception(output);
            }
        }

        public Process StartProcess(string workingdir, string filename, string arguments)
        {
            var p = new Process
                {
                    StartInfo =
                        {
                            Arguments = arguments,
                            CreateNoWindow = true,
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            RedirectStandardInput = true,
                            RedirectStandardError = true,
                            FileName = filename
                        }
                };

            // Mono's pedump won't find the dependent assemblies if the output 
            // directory is not in the path. It can also give problems with the 
            // encoding if it's not forced to one.
            if (Type.GetType("Mono.Runtime") != null)
            {
                p.StartInfo.EnvironmentVariables["MONO_PATH"] = workingdir;
                p.StartInfo.StandardOutputEncoding = Encoding.UTF8;
                p.StartInfo.StandardErrorEncoding = Encoding.UTF8;
            }
            p.Start();
            return p;
        }
    }
}