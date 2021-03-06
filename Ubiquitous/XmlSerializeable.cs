﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

namespace Ubiquitous
{

    public class XmlSerializableBase<T> where T : XmlSerializableBase<T>
    {
        static XmlSerializer serializer = new XmlSerializer(typeof(T));
        public static T Deserialize(XmlReader from) { return (T)serializer.Deserialize(from); }
        public void SerializeTo(Stream s) { serializer.Serialize(s, this); }
        public void SerializeTo(TextWriter w) { serializer.Serialize(w, this); }
        public void SerializeTo(XmlWriter xw) { serializer.Serialize(xw, this); }
        public void SerializeTo(StringWriter sw) { serializer.Serialize(sw, this); }
    }

}
