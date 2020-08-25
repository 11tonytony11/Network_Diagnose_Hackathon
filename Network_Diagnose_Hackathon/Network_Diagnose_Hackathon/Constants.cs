namespace Network_Diagnose_Hackathon
{
    class Constants
    {
        // All numeric variables
        public const int LENGTH = 2;
        public const int MAX_TTL = 50;
        public const int TIMEOUT = 5000;
        public const int MAX_LATENCY = 100;
        public const int NUM_OF_TARGETS = 4;

        // SQL default queries
        public const string SQL_TABLE_CREATE_QUERY = "CREATE TABLE IF NOT EXISTS Diag ( name TEXT PRIMARY KEY, router_counter INTEGER, dns_counter INTEGER, trace_counter INTEGER, highest TEXT )";
        public const string SQL_TABLE_OPERATION_ONE = "SELECT router_counter, dns_counter, trace_counter, highest CROSS APPLY (SELECT top 1 counter highest FROM (VALUES('ROUTER_COUNTER', router_counter), ('DNS_COUNTER', dns_counter), ('TRACE_COUNTER', trace_counter)) x(counter, value) ORDER BY value desc) x";

        // Test types
        public const string DNS_FIRST = "dns";
        public const string DEFAULT = "default";
        public const string TRACE_FIRST = "trace";
        public const string GATEWAY_FIRST = "gateway";
    }
}
