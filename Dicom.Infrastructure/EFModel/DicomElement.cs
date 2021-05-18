﻿namespace Dicom.Infrastructure.EFModel
{
    public class DicomElement
    {
        public int ID { get; set; }

        public int DicomInstanceId{ get; set; }

        public string DicomTag { get; set; }

        public string Value { get; set; }

        public DicomInstance DicomInstance { get; set; }
    }
}