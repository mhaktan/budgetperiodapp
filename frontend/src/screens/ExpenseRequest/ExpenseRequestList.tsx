import React, { useState, useCallback, useEffect, useRef } from 'react';
import { useSearchParams } from 'react-router-dom';
import { Button } from '@takeoff-ui/react-spar';
import { dataProvider } from '../../dataProvider';
import { useListQuery } from '../../shared/useListQuery';
import { useDeleteMutation } from '../../shared/useDeleteMutation';
import { DeleteConfirmDialog } from '../../shared/DeleteConfirmDialog';
import { ListPageLayout } from '../../shared/ListPageLayout';
import type { TableColumn } from '../../shared/ListPageLayout';
import { actionColumn } from '../../shared/ActionButtons';
import { ExpenseRequestCreate } from './ExpenseRequestCreate';
import { ExpenseRequestEdit } from './ExpenseRequestEdit';
import { useFlows } from '../../flows/FlowProvider';

// ---------------------------------------------------------------------------
// Types
// ---------------------------------------------------------------------------

type ExpenseRequestRecord = {
  id: string | number;
  title: string;
  amount: number;
  requestDate: string;
  description?: string;
  status: string;
  revisionNote?: string;
  budgetLineId: string;
  expenseCategoryId: string;
  departmentId: string;
  [key: string]: unknown;
};

// ---------------------------------------------------------------------------
// Column definition — edit this array to add/remove/reorder columns
// ---------------------------------------------------------------------------
//
// Override examples:
//   • Hide a column:     remove its entry from COLUMNS
//   • Add a custom col:  { field: 'fullName', header: 'Full Name', html: (row) => `${row.firstName} ${row.lastName}` }
//   • Enable filtering:  add searchable: true  or  filterType: 'text' | 'checkbox' | 'radio' | 'datepicker'
//   • Custom cell render: html: (row) => `<span style="color:green">${row.status}</span>`
//
// Shared components (src/shared/) can be edited to change behavior globally:
//   • ListPageLayout  — table wrapper, pagination, header layout
//   • ActionButtons   — edit/delete button styles, labels, and behavior
//   • useListQuery    — data fetching, sorting, filtering logic
//   • useDeleteMutation / DeleteConfirmDialog — delete flow
//
// Action buttons override: edit src/shared/ActionButtons.ts DEFAULT_CONFIG
// or pass custom config: actionColumn('id', { hasEdit: true, config: { edit: { label: 'View', style: '...' } } })
//

const COLUMNS: TableColumn[] = [
  { field: 'id', header: 'ID', sortable: true },
  { field: 'title', header: "Title", sortable: true, searchable: true, filterType: 'text' },
  { field: 'amount', header: "Amount", sortable: true },
  { field: 'requestDate', header: "Request Date", sortable: true, filterType: 'datepicker', html: (row: Record<string, unknown>) => row.requestDate ? new Date(String(row.requestDate)).toLocaleDateString() : '—' },
  { field: 'description', header: "Description", sortable: true, searchable: true, filterType: 'text' },
  { field: 'status', header: "Status", sortable: true, filterType: 'radio', filterOptions: [{ label: "Draft", value: "0" }, { label: "PendingManagerApproval", value: "1" }, { label: "PendingFinanceApproval", value: "2" }, { label: "Approved", value: "3" }, { label: "Revision", value: "4" }, { label: "Rejected", value: "5" }, { label: "Cancelled", value: "6" }], html: (row: Record<string, unknown>) => { const m: Record<string, string> = {["0"]: "Draft", ["1"]: "PendingManagerApproval", ["2"]: "PendingFinanceApproval", ["3"]: "Approved", ["4"]: "Revision", ["5"]: "Rejected", ["6"]: "Cancelled"}; return m[String(row.status ?? '')] ?? String(row.status ?? '\u2014'); } },
  { field: 'revisionNote', header: "Revision Note", sortable: true, searchable: true, filterType: 'text' },
  { field: 'budgetLineId', header: "Bütçe Kalemi", sortable: true },
  { field: 'expenseCategoryId', header: "Harcama Kategorisi", sortable: true },
  { field: 'departmentId', header: "Departman", sortable: true },
];

// ---------------------------------------------------------------------------
// ExpenseRequestList
// ---------------------------------------------------------------------------

export const ExpenseRequestList: React.FC = () => {
  const list = useListQuery<ExpenseRequestRecord>({ resource: 'ExpenseRequest' });
  const [showCreate, setShowCreate] = useState(false);
  const [editRecord, setEditRecord] = useState<ExpenseRequestRecord | null>(null);
  const [selectedRows, setSelectedRows] = useState<ExpenseRequestRecord[]>([]);

  // ?focus=<id> ile gelindiginde kayit dogrudan acilir (My Tasks -> kayit)
  const [searchParams, setSearchParams] = useSearchParams();
  const focusId = searchParams.get('focus');
  const focusedRef = useRef<string | null>(null);
  useEffect(() => {
    if (!focusId || focusedRef.current === focusId) return;
    focusedRef.current = focusId;
    const clear = () => {
      const next = new URLSearchParams(searchParams);
      next.delete('focus');
      setSearchParams(next, { replace: true });
    };
    Promise.resolve(dataProvider.getOne('ExpenseRequest', focusId))
      .then((rec) => { if (rec) setEditRecord(rec as ExpenseRequestRecord); })
      .catch(() => { /* kayit bulunamadi — liste normal acilir */ })
      .finally(clear);
  }, [focusId]);
  const { triggerFlows } = useFlows();
  const del = useDeleteMutation('ExpenseRequest', (ids) => { setSelectedRows([]); ids.forEach(id => triggerFlows('delete', 'ExpenseRequest', { id })); });


  const columns = [...COLUMNS, ...actionColumn('id', { hasEdit: true, hasDelete: true })];

  const handleCrudAction = useCallback((action: string, id: string) => {
    const row = list.records.find((r) => String(r.id) === id);
    if (!row) return;
    if (action === 'edit') setEditRecord(row);
    if (action === 'delete') del.requestSingleDelete(row.id);

  }, [list.records]);

  return (
    <>
      <ListPageLayout
        title="ExpenseRequest"
        subtitle={Object.keys(list.displayParams).length > 0 ? (
          <div style={{ fontSize: 13, color: '#666', marginTop: 4 }}>
            {Object.entries(list.displayParams).map(([k, v]) => (
              <span key={k} style={{ marginRight: 12 }}>{k}: <strong>{v}</strong></span>
            ))}
          </div>
        ) : undefined}
        records={list.records}
        columns={columns}
        dataKey="id"
        total={list.total}
        loading={list.isLoading}
        page={list.page}
        perPage={list.perPage}
        onPageChange={list.setPage}
        onPerPageChange={list.setPerPage}
        onTableRequest={list.handleTableRequest}
        selectionMode="checkbox"
        selectedRows={selectedRows}
        onSelectionChange={(rows) => setSelectedRows(rows as ExpenseRequestRecord[])}
        onCrudAction={handleCrudAction}
        headerActions={<>
          {selectedRows.length > 0 && (
            <Button variant="danger" appearance="filled" onClick={() => del.requestDelete(selectedRows.map(r => r.id), `${selectedRows.length} record(s)`)}>{`Delete (${selectedRows.length})`}</Button>
          )}

          <Button variant="primary" appearance="filled" onClick={() => setShowCreate(true)}>+ Create ExpenseRequest</Button>
        </>}
      />

      {list.isError && (
        <div style={{ padding: '10px 14px', background: '#fff3f3', border: '1px solid #f5c6c6', borderRadius: 6, color: '#c62828', fontSize: 13, marginBottom: 12 }}>
          Failed to load data: {(list.error as Error).message}
        </div>
      )}

      <DeleteConfirmDialog
        visible={!!del.deleteTarget}
        label={del.deleteTarget?.label ?? ''}
        isPending={del.isPending}
        onConfirm={del.confirmDelete}
        onCancel={() => del.setDeleteTarget(null)}
      />
      <ExpenseRequestCreate open={showCreate} onClose={() => setShowCreate(false)} onSuccess={list.invalidate} />
      <ExpenseRequestEdit record={editRecord} onClose={() => setEditRecord(null)} onSuccess={list.invalidate} />

    </>
  );
};

