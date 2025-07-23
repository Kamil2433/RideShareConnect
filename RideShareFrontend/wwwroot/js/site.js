// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener('DOMContentLoaded', function () {
    // Smooth scrolling for anchor links
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
            e.preventDefault();
            const target = document.querySelector(this.getAttribute('href'));
            if (target) {
                target.scrollIntoView({
                    behavior: 'smooth'
                });
            }
        });
    });

    // Add click handlers for auth buttons
    const loginBtn = document.querySelector('.btn-login');
    const signupBtn = document.querySelector('.btn-signup');

    loginBtn.addEventListener('click', function (e) {
        e.preventDefault();
        alert('Login functionality would be implemented here');
    });

    signupBtn.addEventListener('click', function (e) {
        e.preventDefault();
        alert('Sign up functionality would be implemented here');
    });

    // Add click handlers for CTA buttons
    const findRideBtn = document.querySelector('.btn-primary');
    const offerRideBtn = document.querySelector('.btn-secondary');

    findRideBtn.addEventListener('click', function (e) {
        e.preventDefault();
        alert('Find a ride functionality would be implemented here');
    });

    offerRideBtn.addEventListener('click', function (e) {
        e.preventDefault();
        alert('Offer a ride functionality would be implemented here');
    });
});

