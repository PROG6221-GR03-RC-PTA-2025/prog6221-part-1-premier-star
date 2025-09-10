using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace CMCS
{
    public partial class MainWindow : Window
    {
        private List<Claim> claims = new List<Claim>();
        private int claimCounter = 1;

        public MainWindow()
        {
            InitializeComponent();
            RefreshUI();
        }

        // --- Lecturer actions ---
        private void SubmitClaim_Click(object sender, RoutedEventArgs e)
        {
            var claim = new Claim
            {
                ClaimId = $"C{claimCounter++:000}",
                LecturerName = "Lecturer A",
                Status = "Submitted",
                LastUpdated = DateTime.Now
            };

            claims.Add(claim);
            MessageBox.Show("Claim submitted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            RefreshUI();
        }

        private void UploadDocument_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Title = "Select Supporting Document";
            if (openFile.ShowDialog() == true)
            {
                MessageBox.Show($"Document uploaded: {openFile.FileName}", "Upload", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // --- Coordinator actions ---
        private void VerifyClaim_Click(object sender, RoutedEventArgs e)
        {
            if (CoordinatorClaimList.SelectedItem is Claim selectedClaim)
            {
                selectedClaim.Status = "Verified";
                selectedClaim.LastUpdated = DateTime.Now;
                MessageBox.Show("Claim verified successfully!", "Verified", MessageBoxButton.OK, MessageBoxImage.Information);
                RefreshUI();
            }
        }

        // --- Manager actions ---
        private void ApproveClaim_Click(object sender, RoutedEventArgs e)
        {
            if (ManagerClaimList.SelectedItem is Claim selectedClaim)
            {
                selectedClaim.Status = "Approved";
                selectedClaim.LastUpdated = DateTime.Now;
                MessageBox.Show("Claim approved successfully!", "Approved", MessageBoxButton.OK, MessageBoxImage.Information);
                RefreshUI();
            }
        }

        // --- Refresh all lists ---
        private void RefreshUI()
        {
            LecturerClaimList.ItemsSource = null;
            CoordinatorClaimList.ItemsSource = null;
            ManagerClaimList.ItemsSource = null;

            LecturerClaimList.ItemsSource = claims;
            CoordinatorClaimList.ItemsSource = claims.Where(c => c.Status == "Submitted").ToList();
            ManagerClaimList.ItemsSource = claims.Where(c => c.Status == "Verified").ToList();
        }
    }

    // Simple Claim model
    public class Claim
    {
        public string ClaimId { get; set; }
        public string LecturerName { get; set; }
        public string Status { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
