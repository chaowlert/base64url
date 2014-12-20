base64url
=========

Library for url-compatible base64 encoding/decoding


####1. Basic encoding/decoding
basic encoding/decoding from string
```
var base64Text = Base64.GetBase64("Text to encode");
var decodedText = Base64.ToString(base64Text);
```

####2. New id
this will create base64 text from Guid
```
var id = Base64.NewId();
```

####3. Sortable id
use TimeId to create ascending/descending id
```
var asc = TimeId.NewSortableId(ascending: true);
var desc = TimeId.NewSortableId();
```

####4. Writer/Reader
This is for writing/reading multiple values
```
var writer = new Base64Writer();
writer.Write(1);
writer.WriteVar("Text");
var base64 = writer.ToString();

var reader = new Base64Reader(base64);
var i = reader.ReadInt32();
var s = reader.ReadVarString();
```

####5. Encryptor/Descryptor
You can also encrypt base64 text, this will use MachineKey for encryption/decryption
```
var writer = new Base64Encryptor("test");
writer.Write(1);
writer.WriteVar("Text");
var base64 = writer.ToString();

var reader = new Base64Decryptor(base64, "test");
var i = reader.ReadInt32();
var s = reader.ReadVarString();
```
