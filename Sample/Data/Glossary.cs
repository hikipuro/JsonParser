using Hikipuro.Text;
using System;

namespace JsonParser.Sample.Data {
	class Glossary {
		public string title = string.Empty;
		public long testInt = 0;
		public float testFloat = 0;
		public double testDouble = 0;
		public string testString = string.Empty;
		public bool testBool = false;
		public DateTime testDate = DateTime.MinValue;
		public string dummy = string.Empty;
		public GlossDiv GlossDiv = null;

		public override string ToString() {
			return ToStringBuilder.Build<Glossary>(this);
		}
	}
}
