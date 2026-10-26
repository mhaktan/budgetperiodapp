import React from 'react';
import { UiCard } from '../../shared/ui';

import {
  ResponsiveContainer,
  LineChart, Line,
  BarChart, Bar,
  PieChart, Pie, Cell,
  CartesianGrid, XAxis, YAxis, Tooltip,
} from 'recharts';

import { API_BASE } from '../../config';
import { getRequestHeaders } from '../../dataProvider';


const LOADING_KEYFRAMES = `
@keyframes pulse { 0%, 100% { opacity: 1; } 50% { opacity: 0.4; } }
@keyframes spin { to { transform: rotate(360deg); } }
`;

export const DashboardScreen: React.FC = () => {
  const getNestedValue = (obj: Record<string, unknown>, path: string): unknown => {
    const direct = path.split('.').reduce<unknown>((o, k) => (o && typeof o === 'object') ? (o as Record<string, unknown>)[k] : undefined, obj);
    if (direct !== undefined) return direct;
    // Fallback: try last segment at top level (handles ABP-style {result: {...}} unwrapping)
    const segments = path.split('.');
    if (segments.length > 1 && obj && typeof obj === "object") {
      const lastKey = segments[segments.length - 1];
      const top = (obj as Record<string, unknown>)[lastKey];
      if (top !== undefined) return top;
    }
    return undefined;
  };

  // Unwrap common API envelopes: ABP {result, __abp}, generic {data}, etc.
  const unwrapResponse = (json: unknown): unknown => {
    if (!json || typeof json !== "object" || Array.isArray(json)) return json;
    const obj = json as Record<string, unknown>;
    // ABP envelope: {result, success, error, __abp}
    if ("__abp" in obj && "result" in obj) return obj.result;
    // Generic envelope: {success: true, data: X}
    if ("success" in obj && "data" in obj && Object.keys(obj).length <= 4) return obj.data;
    return json;
  };

  const extractArray = (json: unknown): Record<string, unknown>[] => {
    if (Array.isArray(json)) return json;
    if (json && typeof json === 'object') {
      const obj = json as Record<string, unknown>;
      for (const key of ['items', 'data', 'results', 'records', 'rows', 'list']) {
        if (Array.isArray(obj[key])) return obj[key] as Record<string, unknown>[];
      }
      // Recurse one level — handles {result: {items: [...]}}
      for (const val of Object.values(obj)) {
        if (val && typeof val === 'object' && !Array.isArray(val)) {
          const inner = val as Record<string, unknown>;
          for (const key of ['items', 'data', 'results', 'records', 'rows', 'list']) {
            if (Array.isArray(inner[key])) return inner[key] as Record<string, unknown>[];
          }
        }
        if (Array.isArray(val)) return val as Record<string, unknown>[];
      }
    }
    return [];
  };

  const [stat_cardData, setStat_cardData] = React.useState<Record<string, unknown> | null>(null);
  const [stat_cardLoading, setStat_cardLoading] = React.useState(true);
  React.useEffect(() => {
    const fetchData = async () => {
      try {
        const res = await fetch(`${API_BASE}/api/services/app/Budget/GetAll?Status=1&MaxResultCount=1`, { headers: getRequestHeaders() });
        // Token expired or invalid — clear auth and redirect to login
        if (res.status === 401) {
          ['_auth_token', '_bearer_token', '_refresh_token'].forEach(k => localStorage.removeItem(k));
          if (window.location.pathname !== '/login') window.location.href = '/login';
          return;
        }
        if (!res.ok) return;
        const rawJson = await res.json();
        // Auto-unwrap ABP/generic envelopes so {result.totalCount} or bare {totalCount} both work
        const json = unwrapResponse(rawJson) as Record<string, unknown>;
        setStat_cardData(json);
      } catch { /* ignore */ }
      finally { setStat_cardLoading(false); }
    };
    fetchData();
  }, []);

  const [stat_card_2Data, setStat_card_2Data] = React.useState<Record<string, unknown> | null>(null);
  const [stat_card_2Loading, setStat_card_2Loading] = React.useState(true);
  React.useEffect(() => {
    const fetchData = async () => {
      try {
        const res = await fetch(`${API_BASE}/api/services/app/Budget/GetAll?Status=2&MaxResultCount=1`, { headers: getRequestHeaders() });
        // Token expired or invalid — clear auth and redirect to login
        if (res.status === 401) {
          ['_auth_token', '_bearer_token', '_refresh_token'].forEach(k => localStorage.removeItem(k));
          if (window.location.pathname !== '/login') window.location.href = '/login';
          return;
        }
        if (!res.ok) return;
        const rawJson = await res.json();
        // Auto-unwrap ABP/generic envelopes so {result.totalCount} or bare {totalCount} both work
        const json = unwrapResponse(rawJson) as Record<string, unknown>;
        setStat_card_2Data(json);
      } catch { /* ignore */ }
      finally { setStat_card_2Loading(false); }
    };
    fetchData();
  }, []);

  const [stat_card_3Data, setStat_card_3Data] = React.useState<Record<string, unknown> | null>(null);
  const [stat_card_3Loading, setStat_card_3Loading] = React.useState(true);
  React.useEffect(() => {
    const fetchData = async () => {
      try {
        const res = await fetch(`${API_BASE}/api/services/app/Approval/GetMyPendingApprovals`, { headers: getRequestHeaders() });
        // Token expired or invalid — clear auth and redirect to login
        if (res.status === 401) {
          ['_auth_token', '_bearer_token', '_refresh_token'].forEach(k => localStorage.removeItem(k));
          if (window.location.pathname !== '/login') window.location.href = '/login';
          return;
        }
        if (!res.ok) return;
        const rawJson = await res.json();
        // Auto-unwrap ABP/generic envelopes so {result.totalCount} or bare {totalCount} both work
        const json = unwrapResponse(rawJson) as Record<string, unknown>;
        setStat_card_3Data(json);
      } catch { /* ignore */ }
      finally { setStat_card_3Loading(false); }
    };
    fetchData();
    const timer = setInterval(fetchData, 30000);
    return () => clearInterval(timer);
  }, []);

  const [stat_card_4Data, setStat_card_4Data] = React.useState<Record<string, unknown> | null>(null);
  const [stat_card_4Loading, setStat_card_4Loading] = React.useState(true);
  React.useEffect(() => {
    const fetchData = async () => {
      try {
        const res = await fetch(`${API_BASE}/api/services/app/ExpenseRequest/GetAll?Status=1&MaxResultCount=1`, { headers: getRequestHeaders() });
        // Token expired or invalid — clear auth and redirect to login
        if (res.status === 401) {
          ['_auth_token', '_bearer_token', '_refresh_token'].forEach(k => localStorage.removeItem(k));
          if (window.location.pathname !== '/login') window.location.href = '/login';
          return;
        }
        if (!res.ok) return;
        const rawJson = await res.json();
        // Auto-unwrap ABP/generic envelopes so {result.totalCount} or bare {totalCount} both work
        const json = unwrapResponse(rawJson) as Record<string, unknown>;
        setStat_card_4Data(json);
      } catch { /* ignore */ }
      finally { setStat_card_4Loading(false); }
    };
    fetchData();
  }, []);

  const [breakdown_budget_4Data, setBreakdown_budget_4Data] = React.useState<Record<string, unknown>[]>([]);
  const [breakdown_budget_4Loading, setBreakdown_budget_4Loading] = React.useState(true);
  React.useEffect(() => {
    const fetchData = async () => {
      try {
        const res = await fetch(`${API_BASE}/api/services/app/Budget/GetGroupedCount?GroupBy=Status`, { headers: getRequestHeaders() });
        // Token expired or invalid — clear auth and redirect to login
        if (res.status === 401) {
          ['_auth_token', '_bearer_token', '_refresh_token'].forEach(k => localStorage.removeItem(k));
          if (window.location.pathname !== '/login') window.location.href = '/login';
          return;
        }
        if (!res.ok) return;
        const rawJson = await res.json();
        // Auto-unwrap ABP/generic envelopes so {result.totalCount} or bare {totalCount} both work
        const json = unwrapResponse(rawJson) as Record<string, unknown>;
        const target = getNestedValue(rawJson as Record<string, unknown>, 'result') ?? getNestedValue(json, 'result');
        setBreakdown_budget_4Data(extractArray(target ?? json));
      } catch { /* ignore */ }
      finally { setBreakdown_budget_4Loading(false); }
    };
    fetchData();
  }, []);

  const [breakdown_expenseRequest_5Data, setBreakdown_expenseRequest_5Data] = React.useState<Record<string, unknown>[]>([]);
  const [breakdown_expenseRequest_5Loading, setBreakdown_expenseRequest_5Loading] = React.useState(true);
  React.useEffect(() => {
    const fetchData = async () => {
      try {
        const res = await fetch(`${API_BASE}/api/services/app/ExpenseRequest/GetGroupedCount?GroupBy=Status`, { headers: getRequestHeaders() });
        // Token expired or invalid — clear auth and redirect to login
        if (res.status === 401) {
          ['_auth_token', '_bearer_token', '_refresh_token'].forEach(k => localStorage.removeItem(k));
          if (window.location.pathname !== '/login') window.location.href = '/login';
          return;
        }
        if (!res.ok) return;
        const rawJson = await res.json();
        // Auto-unwrap ABP/generic envelopes so {result.totalCount} or bare {totalCount} both work
        const json = unwrapResponse(rawJson) as Record<string, unknown>;
        const target = getNestedValue(rawJson as Record<string, unknown>, 'result') ?? getNestedValue(json, 'result');
        setBreakdown_expenseRequest_5Data(extractArray(target ?? json));
      } catch { /* ignore */ }
      finally { setBreakdown_expenseRequest_5Loading(false); }
    };
    fetchData();
  }, []);

  const [breakdown_expenseRequest_6Data, setBreakdown_expenseRequest_6Data] = React.useState<Record<string, unknown>[]>([]);
  const [breakdown_expenseRequest_6Loading, setBreakdown_expenseRequest_6Loading] = React.useState(true);
  React.useEffect(() => {
    const fetchData = async () => {
      try {
        const res = await fetch(`${API_BASE}/api/services/app/ExpenseRequest/GetGroupedCount?GroupBy=ExpenseCategoryId`, { headers: getRequestHeaders() });
        // Token expired or invalid — clear auth and redirect to login
        if (res.status === 401) {
          ['_auth_token', '_bearer_token', '_refresh_token'].forEach(k => localStorage.removeItem(k));
          if (window.location.pathname !== '/login') window.location.href = '/login';
          return;
        }
        if (!res.ok) return;
        const rawJson = await res.json();
        // Auto-unwrap ABP/generic envelopes so {result.totalCount} or bare {totalCount} both work
        const json = unwrapResponse(rawJson) as Record<string, unknown>;
        const target = getNestedValue(rawJson as Record<string, unknown>, 'result') ?? getNestedValue(json, 'result');
        setBreakdown_expenseRequest_6Data(extractArray(target ?? json));
      } catch { /* ignore */ }
      finally { setBreakdown_expenseRequest_6Loading(false); }
    };
    fetchData();
  }, []);

  const [breakdown_budget_7Data, setBreakdown_budget_7Data] = React.useState<Record<string, unknown>[]>([]);
  const [breakdown_budget_7Loading, setBreakdown_budget_7Loading] = React.useState(true);
  React.useEffect(() => {
    const fetchData = async () => {
      try {
        const res = await fetch(`${API_BASE}/api/services/app/Budget/GetGroupedCount?GroupBy=DepartmentId`, { headers: getRequestHeaders() });
        // Token expired or invalid — clear auth and redirect to login
        if (res.status === 401) {
          ['_auth_token', '_bearer_token', '_refresh_token'].forEach(k => localStorage.removeItem(k));
          if (window.location.pathname !== '/login') window.location.href = '/login';
          return;
        }
        if (!res.ok) return;
        const rawJson = await res.json();
        // Auto-unwrap ABP/generic envelopes so {result.totalCount} or bare {totalCount} both work
        const json = unwrapResponse(rawJson) as Record<string, unknown>;
        const target = getNestedValue(rawJson as Record<string, unknown>, 'result') ?? getNestedValue(json, 'result');
        setBreakdown_budget_7Data(extractArray(target ?? json));
      } catch { /* ignore */ }
      finally { setBreakdown_budget_7Loading(false); }
    };
    fetchData();
  }, []);

  return (
    <div>
      <style dangerouslySetInnerHTML={{ __html: LOADING_KEYFRAMES }} />
      <h1 style={{ margin: '0 0 24px', fontSize: 22, fontWeight: 700 }}>Dashboard</h1>
      <div style={{ display: 'grid', gridTemplateColumns: 'repeat(12, 1fr)', gap: 16 }}>
        <div style={{ gridColumn: 'span 6' }}>
          {stat_cardLoading ? (
            <UiCard bodyStyle={{ padding: 20 }}>
              <div style={{ height: 12, width: '40%', background: '#e0e0e0', borderRadius: 4, marginBottom: 12, animation: 'pulse 1.5s ease-in-out infinite' }} />
              <div style={{ height: 28, width: '60%', background: '#e0e0e0', borderRadius: 4, marginBottom: 8, animation: 'pulse 1.5s ease-in-out infinite' }} />
              <div style={{ height: 10, width: '50%', background: '#f0f0f0', borderRadius: 4, animation: 'pulse 1.5s ease-in-out infinite' }} />
            </UiCard>
          ) : (
          <UiCard bodyStyle={{ padding: 20 }}>
            <div style={{ fontSize: 12, color: '#888', marginBottom: 4 }}>Onay Bekleyen Bütçeler</div>
            <div style={{ fontSize: 28, fontWeight: 700, color: '#1976d2' }}>{(getNestedValue(stat_cardData ?? {}, 'result.totalCount') as string | number) ?? '—'}</div>
          </UiCard>
          )}

        </div>
        <div style={{ gridColumn: 'span 6' }}>
          {stat_card_2Loading ? (
            <UiCard bodyStyle={{ padding: 20 }}>
              <div style={{ height: 12, width: '40%', background: '#e0e0e0', borderRadius: 4, marginBottom: 12, animation: 'pulse 1.5s ease-in-out infinite' }} />
              <div style={{ height: 28, width: '60%', background: '#e0e0e0', borderRadius: 4, marginBottom: 8, animation: 'pulse 1.5s ease-in-out infinite' }} />
              <div style={{ height: 10, width: '50%', background: '#f0f0f0', borderRadius: 4, animation: 'pulse 1.5s ease-in-out infinite' }} />
            </UiCard>
          ) : (
          <UiCard bodyStyle={{ padding: 20 }}>
            <div style={{ fontSize: 12, color: '#888', marginBottom: 4 }}>Onaylanan Bütçeler</div>
            <div style={{ fontSize: 28, fontWeight: 700, color: '#1976d2' }}>{(getNestedValue(stat_card_2Data ?? {}, 'result.totalCount') as string | number) ?? '—'}</div>
          </UiCard>
          )}

        </div>
        <div style={{ gridColumn: 'span 6' }}>
          {stat_card_3Loading ? (
            <UiCard bodyStyle={{ padding: 20 }}>
              <div style={{ height: 12, width: '40%', background: '#e0e0e0', borderRadius: 4, marginBottom: 12, animation: 'pulse 1.5s ease-in-out infinite' }} />
              <div style={{ height: 28, width: '60%', background: '#e0e0e0', borderRadius: 4, marginBottom: 8, animation: 'pulse 1.5s ease-in-out infinite' }} />
              <div style={{ height: 10, width: '50%', background: '#f0f0f0', borderRadius: 4, animation: 'pulse 1.5s ease-in-out infinite' }} />
            </UiCard>
          ) : (
          <UiCard bodyStyle={{ padding: 20 }}>
            <div style={{ fontSize: 12, color: '#888', marginBottom: 4 }}>Onayımı Bekleyen Talepler</div>
            <div style={{ fontSize: 28, fontWeight: 700, color: '#1976d2' }}>{Array.isArray((Array.isArray(stat_card_3Data) ? stat_card_3Data as unknown[] : stat_card_3Data?.['result'])) ? ((Array.isArray(stat_card_3Data) ? stat_card_3Data as unknown[] : stat_card_3Data?.['result']) as unknown[]).length : (((Array.isArray(stat_card_3Data) ? stat_card_3Data as unknown[] : stat_card_3Data?.['result']) as string | number) ?? '—')}</div>
          </UiCard>
          )}

        </div>
        <div style={{ gridColumn: 'span 6' }}>
          {stat_card_4Loading ? (
            <UiCard bodyStyle={{ padding: 20 }}>
              <div style={{ height: 12, width: '40%', background: '#e0e0e0', borderRadius: 4, marginBottom: 12, animation: 'pulse 1.5s ease-in-out infinite' }} />
              <div style={{ height: 28, width: '60%', background: '#e0e0e0', borderRadius: 4, marginBottom: 8, animation: 'pulse 1.5s ease-in-out infinite' }} />
              <div style={{ height: 10, width: '50%', background: '#f0f0f0', borderRadius: 4, animation: 'pulse 1.5s ease-in-out infinite' }} />
            </UiCard>
          ) : (
          <UiCard bodyStyle={{ padding: 20 }}>
            <div style={{ fontSize: 12, color: '#888', marginBottom: 4 }}>Onay Bekleyen Harcama Talepleri</div>
            <div style={{ fontSize: 28, fontWeight: 700, color: '#1976d2' }}>{(getNestedValue(stat_card_4Data ?? {}, 'result.totalCount') as string | number) ?? '—'}</div>
          </UiCard>
          )}

        </div>
        <div style={{ gridColumn: 'span 6' }}>
          {breakdown_budget_4Loading ? (
            <UiCard header="Bütçe Durumu Dağılımı" bodyStyle={{ padding: 16, display: 'flex', alignItems: 'center', justifyContent: 'center', height: 280 }}>
              <div style={{ display: 'flex', flexDirection: 'column', alignItems: 'center', gap: 12 }}>
                <div style={{ width: 32, height: 32, border: '3px solid #e0e0e0', borderTopColor: '#1976d2', borderRadius: '50%', animation: 'spin 0.8s linear infinite' }} />
                <span style={{ fontSize: 12, color: '#999' }}>Loading...</span>
              </div>
            </UiCard>
          ) : (
          <UiCard header="Bütçe Durumu Dağılımı" bodyStyle={{ padding: 16 }}>
            <ResponsiveContainer width="100%" height={280}>
                  <PieChart>
                    <Pie data={breakdown_budget_4Data} dataKey="count" nameKey="label" cx="50%" cy="50%" outerRadius={80} label>
                      <Cell fill="#1976d2" />
                      <Cell fill="#ff9800" />
                      <Cell fill="#4caf50" />
                      <Cell fill="#e91e63" />
                      <Cell fill="#9c27b0" />
                    </Pie>
                    <Tooltip />
                  </PieChart>
                </ResponsiveContainer>
          </UiCard>
          )}
        </div>
        <div style={{ gridColumn: 'span 6' }}>
          {breakdown_expenseRequest_5Loading ? (
            <UiCard header="Harcama Talebi Durumu Dağılımı" bodyStyle={{ padding: 16, display: 'flex', alignItems: 'center', justifyContent: 'center', height: 280 }}>
              <div style={{ display: 'flex', flexDirection: 'column', alignItems: 'center', gap: 12 }}>
                <div style={{ width: 32, height: 32, border: '3px solid #e0e0e0', borderTopColor: '#1976d2', borderRadius: '50%', animation: 'spin 0.8s linear infinite' }} />
                <span style={{ fontSize: 12, color: '#999' }}>Loading...</span>
              </div>
            </UiCard>
          ) : (
          <UiCard header="Harcama Talebi Durumu Dağılımı" bodyStyle={{ padding: 16 }}>
            <ResponsiveContainer width="100%" height={280}>
                  <BarChart data={breakdown_expenseRequest_5Data}>
                    <CartesianGrid strokeDasharray="3 3" />
                    <XAxis dataKey="label" />
                    <YAxis />
                    <Tooltip />
                    <Bar dataKey="count" fill="#1976d2" />
                  </BarChart>
                </ResponsiveContainer>
          </UiCard>
          )}
        </div>
        <div style={{ gridColumn: 'span 6' }}>
          {breakdown_expenseRequest_6Loading ? (
            <UiCard header="Kategoriye Göre Harcama Talepleri" bodyStyle={{ padding: 16, display: 'flex', alignItems: 'center', justifyContent: 'center', height: 280 }}>
              <div style={{ display: 'flex', flexDirection: 'column', alignItems: 'center', gap: 12 }}>
                <div style={{ width: 32, height: 32, border: '3px solid #e0e0e0', borderTopColor: '#1976d2', borderRadius: '50%', animation: 'spin 0.8s linear infinite' }} />
                <span style={{ fontSize: 12, color: '#999' }}>Loading...</span>
              </div>
            </UiCard>
          ) : (
          <UiCard header="Kategoriye Göre Harcama Talepleri" bodyStyle={{ padding: 16 }}>
            <ResponsiveContainer width="100%" height={280}>
                  <BarChart data={breakdown_expenseRequest_6Data}>
                    <CartesianGrid strokeDasharray="3 3" />
                    <XAxis dataKey="label" />
                    <YAxis />
                    <Tooltip />
                    <Bar dataKey="count" fill="#1976d2" />
                  </BarChart>
                </ResponsiveContainer>
          </UiCard>
          )}
        </div>
        <div style={{ gridColumn: 'span 6' }}>
          {breakdown_budget_7Loading ? (
            <UiCard header="Departmana Göre Bütçeler" bodyStyle={{ padding: 16, display: 'flex', alignItems: 'center', justifyContent: 'center', height: 280 }}>
              <div style={{ display: 'flex', flexDirection: 'column', alignItems: 'center', gap: 12 }}>
                <div style={{ width: 32, height: 32, border: '3px solid #e0e0e0', borderTopColor: '#1976d2', borderRadius: '50%', animation: 'spin 0.8s linear infinite' }} />
                <span style={{ fontSize: 12, color: '#999' }}>Loading...</span>
              </div>
            </UiCard>
          ) : (
          <UiCard header="Departmana Göre Bütçeler" bodyStyle={{ padding: 16 }}>
            <ResponsiveContainer width="100%" height={280}>
                  <BarChart data={breakdown_budget_7Data}>
                    <CartesianGrid strokeDasharray="3 3" />
                    <XAxis dataKey="label" />
                    <YAxis />
                    <Tooltip />
                    <Bar dataKey="count" fill="#1976d2" />
                  </BarChart>
                </ResponsiveContainer>
          </UiCard>
          )}
        </div>
      </div>
    </div>
  );
};
