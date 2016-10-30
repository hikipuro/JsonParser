using Hikipuro.Text;

namespace JsonParser.Sample.Data {
	class GlossaryData {
		public Glossary glossary = null;

		public Glossary Glossary {
			get { return glossary; }
		}

		public override string ToString() {
			return ToStringBuilder.Build<GlossaryData>(this);
		}
	}
}
