﻿using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ImperaPlus.DataAccess;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.Domain.Utilities;
using ImperaPlus.DTO.Tournaments;

namespace ImperaPlus.Application.Tournaments
{
    public interface ITournamentService
    {
        IEnumerable<DTO.Tournaments.TournamentSummary> GetAll();
        IEnumerable<DTO.Tournaments.TournamentSummary> GetAll(DTO.Tournaments.TournamentState state);

        DTO.Tournaments.Tournament Get(Guid tournamentId);

        IEnumerable<DTO.Tournaments.TournamentTeam> GetTeams(Guid tournamentId);

        TournamentTeamSummary CreateTeam(Guid tournamentId, string name, string password);

        TournamentTeamSummary Join(Guid tournamentId);

        void JoinTeam(Guid tournamentId, Guid teamId, string password);
        
        void DeleteTeam(Guid tournamentId, Guid teamId);

        void Leave(Guid tournamentId);

        IEnumerable<DTO.Tournaments.Tournament> GetAllFull();

        void Create(Tournament tournament);

        void Delete(Guid id);
    }

    public class TournamentService : BaseService, ITournamentService
    {
        public TournamentService(IUnitOfWork unitOfWork, IUserProvider userProvider)
            : base(unitOfWork, userProvider)
        {
        }

        public DTO.Tournaments.TournamentTeamSummary CreateTeam(Guid tournamentId, string name, string password)
        {
            var tournament = this.GetTournament(tournamentId);

            var team = tournament.CreateTeam(this.CurrentUser, name, password);

            this.UnitOfWork.Commit();
           
            return Mapper.Map<TournamentTeamSummary>(team);
        }

        public void DeleteTeam(Guid tournamentId, Guid teamId)
        {
            Require.NotEmpty(tournamentId, nameof(tournamentId));
            Require.NotEmpty(teamId, nameof(teamId));

            var tournament = this.GetTournament(tournamentId);
            var team = this.GetTeam(tournament, teamId);

            tournament.DeleteTeam(this.CurrentUser, team);

            this.UnitOfWork.GetGenericRepository<Domain.Tournaments.TournamentParticipant>().Remove(team.Participants.First());
            this.UnitOfWork.GetGenericRepository<Domain.Tournaments.TournamentTeam>().Remove(team);

            this.UnitOfWork.Commit();            
        }

        public DTO.Tournaments.Tournament Get(Guid tournamentId)
        {
            Require.NotEmpty(tournamentId, nameof(tournamentId));

            Domain.Tournaments.Tournament tournament = GetTournament(tournamentId);

            return Mapper.Map<DTO.Tournaments.Tournament>(tournament);
        }

        public IEnumerable<DTO.Tournaments.TournamentSummary> GetAll(DTO.Tournaments.TournamentState state)
        {
            var domainState = Mapper.Map<Domain.Tournaments.TournamentState>(state);

            return Mapper.Map<IEnumerable<DTO.Tournaments.TournamentSummary>>(this.UnitOfWork.Tournaments.Get(domainState));
        }

        public IEnumerable<DTO.Tournaments.TournamentSummary> GetAll()
        {            
            return Mapper.Map<IEnumerable<DTO.Tournaments.TournamentSummary>>(this.UnitOfWork.Tournaments.Get());
        }

        public IEnumerable<DTO.Tournaments.TournamentTeam> GetTeams(Guid tournamentId)
        {
            Require.NotEmpty(tournamentId, nameof(tournamentId));

            var tournament = this.GetTournament(tournamentId);

            return Mapper.Map<IEnumerable<DTO.Tournaments.TournamentTeam>>(tournament.Teams);
        }

        public TournamentTeamSummary Join(Guid tournamentId)
        {
            Require.NotEmpty(tournamentId, nameof(tournamentId));

            var tournament = this.GetTournament(tournamentId);
            var team = tournament.AddUser(this.CurrentUser);
            
            this.UnitOfWork.Commit();

            return Mapper.Map<TournamentTeamSummary>(team);
        }

        public void JoinTeam(Guid tournamentId, Guid teamId, string password)
        {
            Require.NotEmpty(tournamentId, nameof(tournamentId));

            var tournament = this.GetTournament(tournamentId);
            var team = this.GetTeam(tournament, teamId);

            tournament.AddUser(this.CurrentUser, team, password);

            this.UnitOfWork.Commit();
        }

        public void Leave(Guid tournamentId)
        {
            Require.NotEmpty(tournamentId, nameof(tournamentId));

            var tournament = this.GetTournament(tournamentId);

            var participantToRemove = tournament.LeaveUser(this.CurrentUser);

            this.UnitOfWork.GetGenericRepository<Domain.Tournaments.TournamentParticipant>().Remove(participantToRemove);

            this.UnitOfWork.Commit();
        }

        private Domain.Tournaments.Tournament GetTournament(Guid tournamentId)
        {
            Require.NotEmpty(tournamentId, nameof(tournamentId));

            var tournament = this.UnitOfWork.Tournaments.GetById(tournamentId);
            if (tournament == null)
            {
                throw new Exceptions.ApplicationException("Cannot find tournament", ErrorCode.TournamentNotFound);
            }

            return tournament;
        }

        private Domain.Tournaments.TournamentTeam GetTeam(Domain.Tournaments.Tournament tournament, Guid teamId)
        {
            var team = tournament.Teams.FirstOrDefault(x => x.Id == teamId);
            if (team == null)
            {
                throw new Exceptions.ApplicationException("Cannot find team", ErrorCode.TournamentTeamNotFound);
            }

            return team;
        }

        public void Create(Tournament tournament)
        {
            if (tournament.MapTemplates == null || tournament.MapTemplates.Count() == 0)
            {
                throw new Exceptions.ApplicationException("No map templates", ErrorCode.MapTemplatesRequired);
            }

            var gameOptions = Mapper.Map<Domain.Games.GameOptions>(tournament.Options);

            var newTournament = new Domain.Tournaments.Tournament(
                tournament.Name, tournament.NumberOfTeams, tournament.NumberOfGroupGames, tournament.NumberOfKnockoutGames, 
                tournament.NumberOfFinalGames, tournament.StartOfRegistration, tournament.StartOfTournament, gameOptions);

            foreach (var mapTemplateName in tournament.MapTemplates)
            {
                // Ensure MapTemplates exist
                if (this.UnitOfWork.MapTemplateDescriptors.Get(mapTemplateName) == null)
                {
                    throw new Exceptions.ApplicationException("Cannot find map template", ErrorCode.CannotFindMapTemplate);
                }
            }

            newTournament.MapTemplates.Clear();
            newTournament.MapTemplates.AddRange(tournament.MapTemplates);

            if (this.UnitOfWork.Tournaments.ExistsWithName(tournament.Name))
            {
                throw new Exceptions.ApplicationException("Name is already taken", ErrorCode.CannotCreateTournament);
            }

            this.UnitOfWork.Tournaments.Add(newTournament);
            this.UnitOfWork.Commit();
        }

        public IEnumerable<Tournament> GetAllFull()
        {
            var tournaments = this.UnitOfWork.Tournaments.GetAllFull();

            return Mapper.Map<IEnumerable<DTO.Tournaments.Tournament>>(tournaments);
        }

        public void Delete(Guid tournamentId)
        {
            // TODO: Check Admin

            var tournament = this.GetTournament(tournamentId);

            this.UnitOfWork.Tournaments.Remove(tournament);
            this.UnitOfWork.Commit();
        }
    }
}