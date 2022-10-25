﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UsuariosApi.ModelsWebHook
{
    public class Contact
    {
        public Profile profile { get; set; }
        public string wa_id { get; set; }
    }

    public class Message
    {
        public string from { get; set; }
        public string id { get; set; }
        public string timestamp { get; set; }
        public string type { get; set; }
        public Text text { get; set; }
    }

    public class Metadata
    {
        public string display_phone_number { get; set; }
        public string phone_number_id { get; set; }
    }

    public class Profile
    {
        public string name { get; set; }
    }

    public class Root
    {
        public string field { get; set; }
        public Value value { get; set; }
    }

    public class Text
    {
        public string body { get; set; }
    }

    public class Value
    {
        public string messaging_product { get; set; }
        public Metadata metadata { get; set; }
        public List<Contact> contacts { get; set; }
        public List<Message> messages { get; set; }
    }
}
