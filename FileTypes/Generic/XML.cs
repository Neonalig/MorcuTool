namespace MorcuTool;

public class XML
{
    public enum ReadMode
    {
        WAITING_FOR_TAG,
        READING_TAG
    }

    public readonly List<XMLtag> tags = new();

    public XMLtag GetFirstRootTagWithName(string name)
    {
        for (int i = 0; i < tags.Count; i++)
        {
            if (tags[i].name.ToLower() == name.ToLower())
            {
                return tags[i];
            }
        }

        return null;
    }

    public List<XMLtag> GetAllRootTagsWithName(string name)
    {
        var output = new List<XMLtag>();
        for (int i = 0; i < tags.Count; i++)
        {
            if (tags[i].name.ToLower() == name.ToLower())
            {
                output.Add(tags[i]);
            }
        }

        return output;
    }

    public XML(string input)
    {

        input = input.Trim();
        int pos = 0;
        var readMode = ReadMode.WAITING_FOR_TAG;

        XMLtag currentTag = null;

        int startOfIntraTagData = 0;

        while (pos < input.Length)
        {

            switch (readMode)
            {

                case ReadMode.WAITING_FOR_TAG:

                    if (input[pos] != '<')
                    {
                        pos++;
                    }
                    else
                    {
                        readMode = ReadMode.READING_TAG;

                        if (currentTag != null && pos - startOfIntraTagData > 0)
                        {
                            currentTag.data = input.Substring(startOfIntraTagData, pos - startOfIntraTagData).Trim();
                            if (currentTag.data == "")
                            {
                                currentTag.data = null;
                            }
                        }
                    }

                    break;

                case ReadMode.READING_TAG:

                    pos++; //skip opening '<'
                    int TagDataStartPos = pos;
                    int TagDataLength = 0;
                    bool withinQuotationMarks = false;

                    while (!(input[pos] == '>' && !withinQuotationMarks))
                    {
                        //until we reach the end of the tag and have left quotation marks
                        if (input[pos] == '\"')
                        {
                            withinQuotationMarks = !withinQuotationMarks;
                        }

                        pos++;
                        TagDataLength++;
                    }

                    pos++;

                    //test what kind of tag this is

                    switch (input[TagDataStartPos])
                    {
                        case '?':
                        {
                            currentTag = new XMLtag(input.Substring(TagDataStartPos + 1, TagDataLength - 2), currentTag);

                            if (currentTag.parent == null)
                            {
                                //then we are at the root level
                                tags.Add(currentTag);
                            }
                            else
                            {
                                currentTag.parent.children.Add(currentTag);
                            }

                            currentTag = currentTag.parent; //just go back to the parent because a tag with '?' as the first char never has any children
                            break;
                        }
                        //end the current tag and go back up
                        case '/':
                            currentTag = currentTag.parent;
                            break;
                        default:
                        {
                            bool goUp = false;

                            if (input[TagDataStartPos + (TagDataLength - 1)] == '/')
                            {
                                //a tag with no children
                                currentTag = new XMLtag(input.Substring(TagDataStartPos, TagDataLength - 1), currentTag);
                                goUp = true;
                            }
                            else
                            {
                                currentTag = new XMLtag(input.Substring(TagDataStartPos, TagDataLength), currentTag);
                            }

                            if (currentTag.parent == null)
                            {
                                //then we are at the root level
                                tags.Add(currentTag);
                            }
                            else
                            {
                                currentTag.parent.children.Add(currentTag);
                            }

                            if (goUp)
                            {
                                //this was a one-and-done tag and ended itself, so return to the parent
                                currentTag = currentTag.parent;
                            }

                            break;
                        }
                    }

                    readMode = ReadMode.WAITING_FOR_TAG;
                    startOfIntraTagData = pos;
                    break;
            }
        }

        Console.WriteLine("Finished parsing XML");
    }


    public class XMLtag
    {

        public readonly string name;
        public readonly ParamKeyValuePair[] myParams;
        public string data;

        public readonly XMLtag parent;
        public readonly List<XMLtag> children = new();

        public XMLtag(string _params, XMLtag _parent)
        {
            bool withinQuotationMarks = false;

            int previousSplitPosition = 0;

            var rawParams = new List<string>();

            for (int i = 0; i < _params.Length; i++)
            {

                if (_params[i] == '\"' && !(i > 0 && _params[i - 1] == '\\'))
                {
                    withinQuotationMarks = !withinQuotationMarks;
                }

                if (!withinQuotationMarks && _params[i] == ' ')
                {
                    rawParams.Add(_params.Substring(previousSplitPosition, i - previousSplitPosition));
                    previousSplitPosition = i;
                }
            }

            rawParams.Add(_params.Substring(previousSplitPosition, _params.Length - previousSplitPosition)); //because otherwise the last one wouldn't be caught

            for (int i = rawParams.Count - 1; i >= 0; i--)
            {

                if (rawParams[i].Trim() == "")
                {
                    rawParams.RemoveAt(i);
                }
            }

            name = rawParams[0];

            parent = _parent;

            /* if (parent != null) {

                  onsole.WriteLine("new tag: " + name + ", child of " + parent.name);



              lse {

                  onsole.WriteLine("new tag: " + name + " at the root level");

              */

            myParams = new ParamKeyValuePair[rawParams.Count - 1];

            for (int i = 1; i < rawParams.Count; i++)
            {
                myParams[i - 1] = new ParamKeyValuePair(rawParams[i]);
                Console.WriteLine(myParams[i - 1].key);
            }
        }

        public class ParamKeyValuePair
        {

            public readonly string key;
            public readonly string value;

            public ParamKeyValuePair(string input)
            {
                string[] splitInput = input.Split('=');
                key = splitInput[0].Trim();
                value = splitInput[1].Trim();
            }
        }

        public string GetParamValue(string paramKey)
        {

            foreach (ParamKeyValuePair param in myParams)
            {
                if (param.key.ToLower() == paramKey.ToLower())
                {
                    if (param.value[0] == '\"')
                    {
                        return param.value.Substring(1, param.value.Length - 2);
                    }

                    return param.value;
                }
            }

            return null;
        }

        public XMLtag GetFirstChildWithParamAndValue(string key, string value)
        {
            for (int i = 0; i < children.Count; i++)
            {
                foreach (ParamKeyValuePair param in children[i].myParams)
                {
                    if (param.key.ToLower() == key.ToLower() && param.value.ToLower() == value.ToLower())
                    {
                        return children[i];
                    }
                }
            }

            return null;
        }

        public XMLtag GetFirstChildWithName(string name)
        {
            for (int i = 0; i < children.Count; i++)
            {
                if (children[i].name.ToLower() == name.ToLower())
                {
                    return children[i];
                }
            }

            return null;
        }

        public List<XMLtag> GetChildrenWithName(string name)
        {
            var output = new List<XMLtag>();
            for (int i = 0; i < children.Count; i++)
            {
                if (children[i].name.ToLower() == name.ToLower())
                {
                    output.Add(children[i]);
                }
            }

            return output;
        }
    }
}