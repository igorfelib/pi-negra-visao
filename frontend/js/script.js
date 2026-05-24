const modal = document.querySelector("#modal");
const abrirBtn = document.querySelector("#abrirBtn");
const fecharBtn = document.querySelector("#fecharBtn");
const inscreverseBtn = document.querySelector("#inscreverseBtn");

abrirBtn.addEventListener("click", () => modal.showModal()); 
fecharBtn.addEventListener("click", () => modal.close());
inscreverseBtn.addEventListener("click", () => window.location.href = "inscricao.html");