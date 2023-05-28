using System;
using System.Collections.Generic;
using System.Drawing;
using TestJeux.API.Services;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.Business.Managers.API
{
	public interface IMoveService : ISubscribingService
    {
		/// <summary>
		/// Register item in list of manager
		/// </summary>
		/// <param name="itemId"></param>
		void Register(int itemId);

		/// <summary>
		/// Unregister item
		/// </summary>
		/// <param name="item"></param>
		void Unregister(int itemId);

		/// <summary>
		/// Check if move is possible
		/// </summary>
		/// <param name="movetype"></param>
		/// <param name="position"></param>
		/// <returns></returns>
		bool CanMove(MoveType movetype, Point position);

		/// <summary>
		/// Check if position is occupied by an item
		/// </summary>
		/// <param name="point"></param>
		/// <returns></returns>
		int IsPositionOccupied(Point point);

		/// <summary>
		/// Check has ground modifier
		/// </summary>
		/// <param name="point"></param>
		/// <returns></returns>
		GroundModifier HasGroundModifier(Point point);

		/// <summary>
		/// Get target position
		/// </summary>
		/// <param name="position"></param>
		/// <param name="direction"></param>
		/// <returns></returns>
		Point GetTargetPosition(Point position, DirectionEnum direction);

		/// <summary>
		/// Move character
		/// </summary>
		/// <param name="direction"></param>
		/// <param name="id"></param>
		void MoveCharacter(DirectionEnum direction, int id);

		/// <summary>
		/// Queue movement for next action
		/// </summary>
		/// <param name="direction"></param>
		/// <param name="id"></param>
		void QueueMove(DirectionEnum direction, int id);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		bool HasQueuedMove(int id);

		/// <summary>
		/// Notify that movement is completed
		/// </summary>
		/// <param name="itemId"></param>
		void NotifyEndOfMove(int itemId);

		/// <summary>
		/// Clear queued move for item
		/// </summary>
		/// <param name="id"></param>
		void ClearQueudMove(int id);

		event EventHandler<MovementDto> MoveStarted;

		/// <summary>
		/// Get available direction from position for a move type 
		/// </summary>
		/// <param name="pos"></param>
		/// <param name="moveType"></param>
		/// <returns></returns>
		List<DirectionEnum> GetAvailableDirections(Point pos, MoveType moveType);
	}
}