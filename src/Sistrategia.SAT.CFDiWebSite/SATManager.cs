using System;
using System.Collections.Generic;
//using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Sistrategia.SAT.CFDiWebSite.Messaging;
using Sistrategia.SAT.CFDiWebSite.Data;

namespace Sistrategia.SAT.CFDiWebSite
{
    public class SATManager
    {

        internal static string NormalizeWhiteSpace(string S) {
            if (string.IsNullOrEmpty(S))
                return S;

            string s = S.Trim();
            bool iswhite = false;
            // int iwhite;
            int sLength = s.Length;
            System.Text.StringBuilder sb = new System.Text.StringBuilder(sLength);
            foreach (char c in s.ToCharArray()) {
                if (Char.IsWhiteSpace(c)) {
                    if (iswhite) {
                        //Continuing whitespace ignore it.
                        continue;
                    }
                    else {
                        //New WhiteSpace

                        //Replace whitespace with a single space.
                        sb.Append(" ");
                        //Set iswhite to True and any following whitespace will be ignored
                        iswhite = true;
                    }
                }
                else {
                    sb.Append(c.ToString());
                    //reset iswhitespace to false
                    iswhite = false;
                }
            }
            return sb.ToString();
        }
    }
}