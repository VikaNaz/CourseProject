//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace КП1
{
    using System;
    using System.Collections.Generic;
    
    public partial class Dialog
    {
        public int ID { get; set; }
        public Nullable<int> MessageId { get; set; }
        public Nullable<int> Sender_ID { get; set; }
        public Nullable<int> Receiver_ID { get; set; }
        public string Receiver_Name { get; set; }
    
        public virtual Messages Messages { get; set; }
    }
}
