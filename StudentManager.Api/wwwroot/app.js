"use strict";

const api = {
  async list() {
    return fetchJson("/api/students");
  },
  async search(name) {
    return fetchJson(`/api/students/search?name=${encodeURIComponent(name)}`);
  },
  async create(student) {
    return fetchJson("/api/students", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(student),
    });
  },
  async update(id, name, enrolmentCompleted) {
    return fetchJson(`/api/students/${encodeURIComponent(id)}`, {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ name, enrolmentCompleted }),
    });
  },
  async remove(id) {
    const response = await fetch(`/api/students/${encodeURIComponent(id)}`, { method: "DELETE" });
    if (!response.ok) throw new Error(await errorMessage(response));
  },
};

async function fetchJson(url, options) {
  const response = await fetch(url, options);
  if (!response.ok) throw new Error(await errorMessage(response));
  return response.json();
}

async function errorMessage(response) {
  try {
    const body = await response.json();
    if (body && body.error) return body.error;
  } catch { /* not JSON */ }
  return `Request failed (${response.status})`;
}

// --- Elements ---
const form = document.getElementById("student-form");
const formTitle = document.getElementById("form-title");
const idInput = document.getElementById("student-id");
const nameInput = document.getElementById("student-name");
const enrolledInput = document.getElementById("student-enrolled");
const saveBtn = document.getElementById("save-btn");
const cancelBtn = document.getElementById("cancel-btn");
const formError = document.getElementById("form-error");
const rowsBody = document.getElementById("student-rows");
const listStatus = document.getElementById("list-status");
const searchInput = document.getElementById("search-input");
const searchBtn = document.getElementById("search-btn");
const clearSearchBtn = document.getElementById("clear-search-btn");

let editingId = null; // null = add mode, otherwise the ID being edited
let activeSearch = null;

// --- Rendering ---
function render(students) {
  rowsBody.replaceChildren();
  for (const student of students) {
    const tr = document.createElement("tr");

    const idCell = document.createElement("td");
    idCell.textContent = student.id;

    const nameCell = document.createElement("td");
    nameCell.textContent = student.name;

    const statusCell = document.createElement("td");
    const badge = document.createElement("span");
    badge.className = `badge ${student.enrolmentCompleted ? "enrolled" : "pending"}`;
    badge.textContent = student.enrolmentCompleted ? "Completed" : "Pending";
    statusCell.appendChild(badge);

    const actionsCell = document.createElement("td");
    const actions = document.createElement("div");
    actions.className = "row-actions";

    const editBtn = document.createElement("button");
    editBtn.className = "secondary";
    editBtn.textContent = "Edit";
    editBtn.addEventListener("click", () => startEdit(student));

    const deleteBtn = document.createElement("button");
    deleteBtn.className = "danger";
    deleteBtn.textContent = "Delete";
    deleteBtn.addEventListener("click", () => deleteStudent(student.id));

    actions.append(editBtn, deleteBtn);
    actionsCell.appendChild(actions);

    tr.append(idCell, nameCell, statusCell, actionsCell);
    rowsBody.appendChild(tr);
  }

  if (students.length === 0) {
    listStatus.textContent = activeSearch
      ? `No students found with name "${activeSearch}".`
      : "No students yet — add one above.";
  } else {
    listStatus.textContent = activeSearch
      ? `${students.length} match(es) for "${activeSearch}".`
      : `${students.length} student(s).`;
  }
}

async function refresh() {
  try {
    const students = activeSearch ? await api.search(activeSearch) : await api.list();
    render(students);
  } catch (err) {
    listStatus.textContent = `Could not load students: ${err.message}`;
  }
}

// --- Form (add / edit) ---
function resetForm() {
  editingId = null;
  form.reset();
  idInput.disabled = false;
  formTitle.textContent = "Add student";
  saveBtn.textContent = "Add";
  cancelBtn.classList.add("hidden");
  formError.classList.add("hidden");
}

function startEdit(student) {
  editingId = student.id;
  idInput.value = student.id;
  idInput.disabled = true;
  nameInput.value = student.name;
  enrolledInput.checked = student.enrolmentCompleted;
  formTitle.textContent = `Edit student ${student.id}`;
  saveBtn.textContent = "Save";
  cancelBtn.classList.remove("hidden");
  formError.classList.add("hidden");
  nameInput.focus();
}

form.addEventListener("submit", async (event) => {
  event.preventDefault();
  formError.classList.add("hidden");
  try {
    if (editingId === null) {
      await api.create({
        id: idInput.value.trim(),
        name: nameInput.value.trim(),
        enrolmentCompleted: enrolledInput.checked,
      });
    } else {
      await api.update(editingId, nameInput.value.trim(), enrolledInput.checked);
    }
    resetForm();
    await refresh();
  } catch (err) {
    formError.textContent = err.message;
    formError.classList.remove("hidden");
  }
});

cancelBtn.addEventListener("click", resetForm);

// --- Delete ---
async function deleteStudent(id) {
  if (!window.confirm(`Delete student ${id}?`)) return;
  try {
    await api.remove(id);
    if (editingId === id) resetForm();
    await refresh();
  } catch (err) {
    listStatus.textContent = `Could not delete ${id}: ${err.message}`;
  }
}

// --- Search ---
async function runSearch() {
  const term = searchInput.value.trim();
  activeSearch = term.length > 0 ? term : null;
  clearSearchBtn.classList.toggle("hidden", activeSearch === null);
  await refresh();
}

searchBtn.addEventListener("click", runSearch);
searchInput.addEventListener("keydown", (event) => {
  if (event.key === "Enter") runSearch();
});
clearSearchBtn.addEventListener("click", async () => {
  searchInput.value = "";
  activeSearch = null;
  clearSearchBtn.classList.add("hidden");
  await refresh();
});

// --- Init ---
refresh();
