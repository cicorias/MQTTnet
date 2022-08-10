using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQTTRelayApp.Models
{
    internal class PayloadFormat
    {
        bool _isBase64String;
        bool _isFilePath;
        bool _isPlainText;

        public PayloadFormat()
        {
            Value = PayloadInputFormat.PlainText;
        }

        public bool IsBase64String
        {
            get => _isBase64String;
            set => _isBase64String = value;
        }

        public bool IsFilePath
        {
            get => _isFilePath;
            set => _isFilePath = value;
        }

        public bool IsPlainText
        {
            get => _isPlainText;
            set => _isPlainText = value;
        }

        public PayloadInputFormat Value
        {
            get
            {
                if (IsPlainText)
                {
                    return PayloadInputFormat.PlainText;
                }

                if (IsBase64String)
                {
                    return PayloadInputFormat.Base64String;
                }

                if (IsFilePath)
                {
                    return PayloadInputFormat.FilePath;
                }

                throw new NotSupportedException();
            }

            set
            {
                IsPlainText = value == PayloadInputFormat.PlainText;
                IsBase64String = value == PayloadInputFormat.Base64String;
                IsFilePath = value == PayloadInputFormat.FilePath;
            }
        }

        public byte[] ConvertPayloadInput(string? payloadInput)
        {
            if (string.IsNullOrEmpty(payloadInput))
            {
                return Array.Empty<byte>();
            }

            if (_isPlainText)
            {
                return Encoding.UTF8.GetBytes(payloadInput);
            }

            if (_isBase64String)
            {
                return Convert.FromBase64String(payloadInput);
            }

            if (_isFilePath)
            {
                return File.ReadAllBytes(payloadInput);
            }

            throw new NotSupportedException();
        }
    }
}
