using UnityEngine;
using System.Collections;

public static class StringTableManager {
	// current string table
	private static StringTable m_table;

	///////////////////////////////////////////
	// Get()
	// Accesses the string value for i_key in
	// the current string table.
	///////////////////////////////////////////
	public static string Get( string i_strKey ) {
		if ( m_table == null )
			m_table = new StringTable("English");

		return m_table.Get( i_strKey );
	}
}
