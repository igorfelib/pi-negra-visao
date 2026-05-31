
const menuToggle = document.querySelector('.menu-toggle');
const siteNav = document.querySelector('#site-nav');

/* =========== MENU AMBURGUER ===========  */
if (menuToggle && siteNav) {
  const closeMobileMenu = () => {
    siteNav.classList.remove('nav-open');
    menuToggle.setAttribute('aria-expanded', 'false');
  };

  menuToggle.addEventListener('click', () => {
    const isExpanded = menuToggle.getAttribute('aria-expanded') === 'true';
    siteNav.classList.toggle('nav-open', !isExpanded);
    menuToggle.setAttribute('aria-expanded', String(!isExpanded));
  });

  siteNav.querySelectorAll('a').forEach((link) => {
    link.addEventListener('click', closeMobileMenu);
  });

  window.addEventListener('resize', () => {
    if (window.innerWidth > 768) {
      closeMobileMenu();
    }
  });
}
