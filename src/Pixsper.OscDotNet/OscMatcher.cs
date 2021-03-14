using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Pixsper.OscDotNet
{
    public class OscMatcher
    {
        private Dictionary<string, HashSet<Action<OscElement>>> _actions = new Dictionary<string, HashSet<Action<OscElement>>>();


        private static readonly Regex IsPatternMatchingRegex = new Regex(@"(\?|\*|\[|\]|{|})", RegexOptions.Compiled);
        private static readonly Dictionary<string, Regex> RegexCache = new Dictionary<string, Regex>();
        private static readonly TimeSpan RegexTimeout = TimeSpan.FromMilliseconds(500);

        public void Match(string addressPattern)
        {
            
        }
        
        private Regex? constructPatternMatchRegex(string address)
        {
            if (RegexCache.TryGetValue(address, out var patternMatch))
                return patternMatch;

            var builder = new StringBuilder(address.Length + 16);
            bool isInCharacterGroup = false;
            bool isInWordGroup = false;
            bool isNextCharacterEscaped = false;

            foreach (char t in address)
            {

                if (isNextCharacterEscaped)
                    builder.Append(t);

                switch (t)
                {
                    // The following are OSC pattern matching special characters which we 
                    // replace with their regex equivalent
                    case '?':
                        builder.Append(@"[^\/]");
                        break;

                    case '*':
                        builder.Append(@"[^\/]*");
                        break;

                    case '[':
                        isInCharacterGroup = true;
                        builder.Append(t);
                        break;

                    case ']':
                        isInCharacterGroup = false;
                        builder.Append(t);
                        break;

                    case '!':
                        builder.Append(isInCharacterGroup ? '^' : t);
                        break;

                    case '{':
                        isInWordGroup = true;
                        builder.Append('(');
                        break;

                    case '}':
                        isInWordGroup = false;
                        builder.Append(')');
                        break;

                    case ',':
                        builder.Append(isInWordGroup ? '|' : t);
                        break;

                    // This allows users to escape OSC special characters in the pattern address
                    case '\\':
                        isNextCharacterEscaped = true;
                        builder.Append(t);
                        break;

                    // The following characters are special in regex but not in OSC so need escaping
                    case '^':
                    case '$':
                    case '.':
                    case '|':
                    case '+':
                    case '(':
                    case ')':
                    case '&':
                    case '/':
                        builder.Append('\\');
                        builder.Append(t);
                        break;

                    default:
                        builder.Append(t);
                        break;
                }
            }

            try
            {
                patternMatch = new Regex(builder.ToString(), RegexOptions.None, RegexTimeout);
            }
            catch (ArgumentException)
            {
                return null;
            }

            RegexCache[address] = patternMatch;

            return patternMatch;
        }
    }
}
